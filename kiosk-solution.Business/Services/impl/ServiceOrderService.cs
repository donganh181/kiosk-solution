using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace kiosk_solution.Business.Services.impl
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IServiceOrderService> _logger;
        private readonly IServiceApplicationService _serviceApplicationService;
        private readonly IKioskService _kioskService;

        public ServiceOrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IServiceOrderService> logger,
            IServiceApplicationService serviceApplicationService, IKioskService kioskService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _serviceApplicationService = serviceApplicationService;
            _kioskService = kioskService;
        }

        public async Task<ServiceOrderViewModel> Create(ServiceOrderCreateViewModel model)
        {

            var isAffiliate = await _serviceApplicationService.GetAffiliateByAppId(model.ServiceApplicationId);
            if (!isAffiliate)
            {
                return null;
            }

            var serviceOrder = _mapper.Map<ServiceOrder>(model);


            dynamic order = JObject.Parse(model.OrderDetail);
            serviceOrder.Total = 0;
            foreach (var item in order.items)
            {
                serviceOrder.Total += Decimal.Parse(item.Price.ToString());
            }

            var appCate = await _serviceApplicationService.GetCommissionById(model.ServiceApplicationId);
            var totalCommission = serviceOrder.Total * Decimal.Parse(appCate.CommissionPercentage + "") / 100;
            

            if (appCate.CommissionPercentage > 1)
            {
                if (totalCommission > 70000)
                {
                    serviceOrder.Commission = 70000 * Decimal.Parse(((appCate.CommissionPercentage - 1)/appCate.CommissionPercentage) + "");
                    serviceOrder.SystemCommission = 70000 - serviceOrder.Commission;
                }
                else
                {
                    serviceOrder.Commission =
                        serviceOrder.Total * Decimal.Parse((appCate.CommissionPercentage - 1) + "") / 100;
                    serviceOrder.SystemCommission = serviceOrder.Total * 1 / 100;
                }
            }
            else
            {
                serviceOrder.Commission = 0;
                if (totalCommission > 70000)
                    serviceOrder.SystemCommission = 70000;
                else
                    serviceOrder.SystemCommission =
                        serviceOrder.Total * Decimal.Parse(appCate.CommissionPercentage + "") / 100;
            }
            
            serviceOrder.KioskId = model.KioskId;
            serviceOrder.ServiceApplicationId = model.ServiceApplicationId;
            serviceOrder.CreateDate = DateTime.Now;
            try
            {
                await _unitOfWork.ServiceOrderRepository.InsertAsync(serviceOrder);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceOrderViewModel>(serviceOrder);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Can not create order.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<ServiceOrderSearchViewModel>> GetAllWithPaging(Guid partyId,
            ServiceOrderSearchViewModel model, int size, int pageNum)
        {
            var kiosk = await _kioskService.GetByIdWithParyId((Guid) model.KioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var orders = _unitOfWork.ServiceOrderRepository.Get(o => o.Kiosk.PartyId.Equals(partyId))
                .ProjectTo<ServiceOrderSearchViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter(model).OrderByDescending(o => o.CreateDate).ThenBy(o => o.ServiceApplicationId);
            var listPaging = orders.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<ServiceOrderSearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommission(Guid partyId,
            ServiceOrderCommissionSearchViewModel model)
        {
            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s => s.Kiosk.PartyId.Equals(partyId))
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).DynamicFilter(model)
                .ToListAsync();
            if (listApp == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.Kiosk.PartyId.Equals(partyId) &&
                              s.ServiceApplicationId.Equals(app.ServiceApplicationId))
                    .SumAsync(o => o.Commission);
                app.TotalCommission = (double) commission;
            }

            return listApp;
        }

        public async Task<DynamicModelResponse<ServiceOrderLocationOwnerViewModel>> GetAllOrderByApplicationWithPaging(
            Guid partyId, Guid serviceApplicationId, int size, int pageNum)
        {
            var listOrder = await _unitOfWork.ServiceOrderRepository
                .Get(o => o.ServiceApplicationId.Equals(serviceApplicationId) && o.Kiosk.PartyId.Equals(partyId))
                .ProjectTo<ServiceOrderLocationOwnerViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(o => o.CreateDate)
                .ToListAsync();
            var orders = listOrder.AsQueryable();
            var listPaging = orders.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<ServiceOrderLocationOwnerViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKiosk(Guid partyId, Guid kioskId)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var result = new ServiceOrderCommissionPieChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new List<decimal>()
            };
            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s => s.KioskId.Equals(kioskId))
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.KioskId.Equals(kioskId) &&
                              s.ServiceApplicationId.Equals(app.ServiceApplicationId))
                    .SumAsync(o => o.Commission);
                result.Datasets.Add((decimal) commission);
                result.Labels.Add(app.ServiceApplicationName);
            }

            return result;
        }

        public async Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKioskByMonth(Guid partyId,
            Guid kioskId,
            int month, int year)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var result = new ServiceOrderCommissionPieChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new List<decimal>()
            };
            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                    s.KioskId.Equals(kioskId) &&
                    s.CreateDate.Value.Month == month &&
                    s.CreateDate.Value.Year == year)
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.KioskId.Equals(kioskId) &&
                              s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                              s.CreateDate.Value.Month == month &&
                              s.CreateDate.Value.Year == year)
                    .SumAsync(o => o.Commission);
                result.Datasets.Add((decimal) commission);
                result.Labels.Add(app.ServiceApplicationName);
            }

            return result;
        }

        public async Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKioskByYear(Guid partyId,
            Guid kioskId,
            int year)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var result = new ServiceOrderCommissionPieChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new List<decimal>()
            };
            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                    s.KioskId.Equals(kioskId) &&
                    s.CreateDate.Value.Year == year)
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.KioskId.Equals(kioskId) &&
                              s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                              s.CreateDate.Value.Year == year)
                    .SumAsync(o => o.Commission);
                result.Datasets.Add((decimal) commission);
                result.Labels.Add(app.ServiceApplicationName);
            }

            return result;
        }

        public async Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionKioskByMonthOfYear(Guid partyId,
            Guid kioskId, int year, List<Guid> serviceApplicationIds)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var result = new ServiceOrderCommissionLineChartViewModel()
            {
                Datas = new List<AppDataViewModel>()
            };
            // Get by list service app ID
            if (serviceApplicationIds.Count != 0)
                foreach (var serviceApplicationId in serviceApplicationIds)
                {
                    if (serviceApplicationId == null)
                    {
                        _logger.LogInformation("App id can not null.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest, "App id can not null.");
                    }

                    var appOrder = await _unitOfWork.ServiceOrderRepository.Get(s =>
                            s.KioskId.Equals(kioskId) &&
                            s.CreateDate.Value.Year == year &&
                            s.ServiceApplicationId == serviceApplicationId)
                        .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (appOrder == null)
                    {
                        var name = await _serviceApplicationService.GetNameById(serviceApplicationId);
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = name
                        };
                        result.Datas.Add(appData);
                    }
                    else
                    {
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = appOrder.ServiceApplicationName
                        };
                        for (var i = 1; i <= 12; i++)
                        {
                            var month = i;
                            var commissionByMonth = new ServiceOrderCommissionSearchViewModel()
                            {
                                TotalCommission = 0,
                                ServiceApplicationId = appOrder.ServiceApplicationId,
                                ServiceApplicationName = appOrder.ServiceApplicationName
                            };
                            var commission = await _unitOfWork.ServiceOrderRepository
                                .Get(s => s.KioskId.Equals(kioskId) &&
                                          s.ServiceApplicationId.Equals(commissionByMonth.ServiceApplicationId) &&
                                          s.CreateDate.Value.Month == month &&
                                          s.CreateDate.Value.Year == year)
                                .SumAsync(o => o.Commission);
                            appData.Datasets.Add((decimal) commission);
                        }

                        result.Datas.Add(appData);
                    }
                }
            // Get all
            else
            {
                var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                        s.KioskId.Equals(kioskId) &&
                        s.CreateDate.Value.Year == year)
                    .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
                foreach (var app in listApp)
                {
                    var appData = new AppDataViewModel()
                    {
                        Datasets = new List<decimal>(),
                        ServiceApplicationId = (Guid) app.ServiceApplicationId,
                        ServiceApplicationName = app.ServiceApplicationName
                    };
                    for (var i = 1; i <= 12; i++)
                    {
                        var month = i;
                        var commission = await _unitOfWork.ServiceOrderRepository
                            .Get(s => s.KioskId.Equals(kioskId) &&
                                      s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                                      s.CreateDate.Value.Month == month &&
                                      s.CreateDate.Value.Year == year)
                            .SumAsync(o => o.Commission);
                        appData.Datasets.Add((decimal) commission);
                    }

                    result.Datas.Add(appData);
                }
            }

            return result;
        }

        public async Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionKioskByDayOfMonth(Guid partyId,
            Guid kioskId, int month, int year, List<Guid> serviceApplicationIds)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);
            var result = new ServiceOrderCommissionLineChartViewModel()
            {
                Datas = new List<AppDataViewModel>()
            };
            // Get by list service app ID
            if (serviceApplicationIds.Count != 0)
                foreach (var serviceApplicationId in serviceApplicationIds)
                {
                    if (serviceApplicationId == null)
                    {
                        _logger.LogInformation("App id can not null.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest, "App id can not null.");
                    }

                    var appOrder = await _unitOfWork.ServiceOrderRepository.Get(s =>
                            s.KioskId.Equals(kioskId) &&
                            s.CreateDate.Value.Month == month &&
                            s.CreateDate.Value.Year == year &&
                            s.ServiceApplicationId == serviceApplicationId)
                        .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (appOrder == null)
                    {
                        var name = await _serviceApplicationService.GetNameById(serviceApplicationId);
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = name
                        };
                        result.Datas.Add(appData);
                    }
                    else
                    {
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = appOrder.ServiceApplicationName
                        };
                        for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                        {
                            var commission = await _unitOfWork.ServiceOrderRepository
                                .Get(s => s.KioskId.Equals(kioskId) &&
                                          s.ServiceApplicationId.Equals(appOrder.ServiceApplicationId) &&
                                          s.CreateDate.Value.Day == dt.Day &&
                                          s.CreateDate.Value.Month == month &&
                                          s.CreateDate.Value.Year == year)
                                .SumAsync(o => o.Commission);
                            appData.Datasets.Add((decimal) commission);
                        }

                        result.Datas.Add(appData);
                    }
                }
            // Get all
            else
            {
                var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                        s.KioskId.Equals(kioskId) &&
                        s.CreateDate.Value.Month == month &&
                        s.CreateDate.Value.Year == year)
                    .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
                foreach (var app in listApp)
                {
                    var appData = new AppDataViewModel()
                    {
                        Datasets = new List<decimal>(),
                        ServiceApplicationId = (Guid) app.ServiceApplicationId,
                        ServiceApplicationName = app.ServiceApplicationName
                    };
                    for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                    {
                        var commission = await _unitOfWork.ServiceOrderRepository
                            .Get(s => s.KioskId.Equals(kioskId) &&
                                      s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                                      s.CreateDate.Value.Day == dt.Day &&
                                      s.CreateDate.Value.Month == month &&
                                      s.CreateDate.Value.Year == year)
                            .SumAsync(o => o.Commission);
                        appData.Datasets.Add((decimal) commission);
                    }

                    result.Datas.Add(appData);
                }
            }

            return result;
        }

        public async Task<ServiceOrderCommissionLineChartByKioskViewModel> GetAllCommissionMonthOfYearByKiosk(
            Guid partyId, int year, List<Guid> kioskIds)

        {
            var result = new ServiceOrderCommissionLineChartByKioskViewModel()
            {
                Datas = new List<KioskDataViewModel>()
            };
            // Get by list service app ID
            if (kioskIds.Count != 0)
                foreach (var kioskId in kioskIds)
                {
                    if (kioskId == null)
                    {
                        _logger.LogInformation("Kiosk id can not null.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Kiosk id can not null.");
                    }

                    var appOrder = await _unitOfWork.ServiceOrderRepository.Get(s =>
                            s.Kiosk.PartyId.Equals(partyId) &&
                            s.KioskId.Equals(kioskId) &&
                            s.CreateDate.Value.Year == year)
                        .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (appOrder == null)
                    {
                        var name = await _kioskService.GetNameById(kioskId);
                        var kioskData = new KioskDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            KioskId = kioskId,
                            KioskName = name
                        };
                        result.Datas.Add(kioskData);
                    }
                    else
                    {
                        var kioskData = new KioskDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            KioskId = kioskId,
                            KioskName = appOrder.KioskName
                        };
                        for (var i = 1; i <= 12; i++)
                        {
                            var month = i;
                            var commission = await _unitOfWork.ServiceOrderRepository
                                .Get(s => s.KioskId.Equals(kioskId) &&
                                          s.Kiosk.PartyId.Equals(partyId) &&
                                          s.CreateDate.Value.Month == month &&
                                          s.CreateDate.Value.Year == year)
                                .SumAsync(o => o.Commission);
                            kioskData.Datasets.Add((decimal) commission);
                        }

                        result.Datas.Add(kioskData);
                    }
                }
            // Get all
            else
            {
                var list = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.CreateDate.Value.Year == year && s.Kiosk.PartyId.Equals(partyId))
                    .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                list = list.GroupBy(o => o.KioskId).Select(g => g.First()).ToList();
                foreach (var order in list)
                {
                    var kioskData = new KioskDataViewModel()
                    {
                        Datasets = new List<decimal>(),
                        KioskId = (Guid) order.KioskId,
                        KioskName = order.KioskName
                    };
                    for (var i = 1; i <= 12; i++)
                    {
                        var month = i;
                        var commission = await _unitOfWork.ServiceOrderRepository
                            .Get(s => s.KioskId.Equals(order.KioskId) &&
                                      s.Kiosk.PartyId.Equals(partyId) &&
                                      s.CreateDate.Value.Month == month &&
                                      s.CreateDate.Value.Year == year)
                            .SumAsync(o => o.Commission);
                        kioskData.Datasets.Add((decimal) commission);
                    }

                    result.Datas.Add(kioskData);
                }
            }

            return result;
        }

        public async Task<ServiceOrderCommissionLineChartByKioskViewModel> GetAllCommissionDayOfMonthByKiosk(
            Guid partyId, int month, int year, List<Guid> kioskIds)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);
            var result = new ServiceOrderCommissionLineChartByKioskViewModel()
            {
                Datas = new List<KioskDataViewModel>()
            };
            // Get by list service app ID
            if (kioskIds.Count != 0)
                foreach (var kioskId in kioskIds)
                {
                    if (kioskId == null)
                    {
                        _logger.LogInformation("Kiosk id can not null.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Kiosk id can not null.");
                    }

                    var kioskOrder = await _unitOfWork.ServiceOrderRepository.Get(s =>
                            s.Kiosk.PartyId.Equals(partyId) &&
                            s.KioskId.Equals(kioskId) &&
                            s.CreateDate.Value.Month == month &&
                            s.CreateDate.Value.Year == year &&
                            s.KioskId == kioskId)
                        .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (kioskOrder == null)
                    {
                        var name = await _kioskService.GetNameById(kioskId);
                        var kioskData = new KioskDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            KioskId = kioskId,
                            KioskName = name
                        };
                        result.Datas.Add(kioskData);
                    }
                    else
                    {
                        var kioskData = new KioskDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            KioskId = kioskId,
                            KioskName = kioskOrder.KioskName
                        };
                        for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                        {
                            var commission = await _unitOfWork.ServiceOrderRepository
                                .Get(s => s.KioskId.Equals(kioskId) &&
                                          s.Kiosk.PartyId.Equals(partyId) &&
                                          s.CreateDate.Value.Day == dt.Day &&
                                          s.CreateDate.Value.Month == month &&
                                          s.CreateDate.Value.Year == year)
                                .SumAsync(o => o.Commission);
                            kioskData.Datasets.Add((decimal) commission);
                        }

                        result.Datas.Add(kioskData);
                    }
                }
            // Get all
            else
            {
                var list = await _unitOfWork.ServiceOrderRepository.Get(s =>
                        s.Kiosk.PartyId.Equals(partyId) &&
                        s.CreateDate.Value.Month == month &&
                        s.CreateDate.Value.Year == year)
                    .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                list = list.GroupBy(o => o.KioskId).Select(g => g.First()).ToList();
                foreach (var order in list)
                {
                    var kioskData = new KioskDataViewModel()
                    {
                        Datasets = new List<decimal>(),
                        KioskId = (Guid) order.KioskId,
                        KioskName = order.KioskName
                    };
                    for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                    {
                        var commission = await _unitOfWork.ServiceOrderRepository
                            .Get(s => s.KioskId.Equals(order.KioskId) &&
                                      s.Kiosk.PartyId.Equals(partyId) &&
                                      s.CreateDate.Value.Day == dt.Day &&
                                      s.CreateDate.Value.Month == month &&
                                      s.CreateDate.Value.Year == year)
                            .SumAsync(o => o.Commission);
                        kioskData.Datasets.Add((decimal) commission);
                    }

                    result.Datas.Add(kioskData);
                }
            }

            return result;
        }

        public async Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionSystemByMonth(int month, int year)
        {
            var result = new ServiceOrderCommissionPieChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new List<decimal>()
            };
            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                    s.CreateDate.Value.Month == month &&
                    s.CreateDate.Value.Year == year)
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s =>
                        s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                        s.CreateDate.Value.Month == month &&
                        s.CreateDate.Value.Year == year)
                    .SumAsync(o => o.SystemCommission);
                result.Datasets.Add((decimal) commission);
                result.Labels.Add(app.ServiceApplicationName);
            }

            return result;
        }

        public async Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionSystemByYear(int year)

        {
            var result = new ServiceOrderCommissionPieChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new List<decimal>()
            };
            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s => s.CreateDate.Value.Year == year)
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s =>
                        s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                        s.CreateDate.Value.Year == year)
                    .SumAsync(o => o.SystemCommission);
                result.Datasets.Add((decimal) commission);
                result.Labels.Add(app.ServiceApplicationName);
            }

            return result;
        }

        public async Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionSystemByMonthOfYear(int year,
            List<Guid> serviceApplicationIds)
        {
            var result = new ServiceOrderCommissionLineChartViewModel()
            {
                Datas = new List<AppDataViewModel>()
            };
            // Get by list service app ID
            if (serviceApplicationIds.Count != 0)
                foreach (var serviceApplicationId in serviceApplicationIds)
                {
                    if (serviceApplicationId == null)
                    {
                        _logger.LogInformation("App id can not null.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest, "App id can not null.");
                    }

                    var appOrder = await _unitOfWork.ServiceOrderRepository.Get(s =>
                            s.CreateDate.Value.Year == year &&
                            s.ServiceApplicationId == serviceApplicationId)
                        .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (appOrder == null)
                    {
                        var name = await _serviceApplicationService.GetNameById(serviceApplicationId);
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = name
                        };
                        result.Datas.Add(appData);
                    }
                    else
                    {
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = appOrder.ServiceApplicationName
                        };
                        for (var i = 1; i <= 12; i++)
                        {
                            var month = i;
                            var commissionByMonth = new ServiceOrderCommissionSearchViewModel()
                            {
                                TotalCommission = 0,
                                ServiceApplicationId = appOrder.ServiceApplicationId,
                                ServiceApplicationName = appOrder.ServiceApplicationName
                            };
                            var commission = await _unitOfWork.ServiceOrderRepository
                                .Get(s =>
                                    s.ServiceApplicationId.Equals(commissionByMonth.ServiceApplicationId) &&
                                    s.CreateDate.Value.Month == month &&
                                    s.CreateDate.Value.Year == year)
                                .SumAsync(o => o.SystemCommission);
                            appData.Datasets.Add((decimal) commission);
                        }

                        result.Datas.Add(appData);
                    }
                }
            // Get all
            else
            {
                var listApp = await _unitOfWork.ServiceOrderRepository.Get(s => s.CreateDate.Value.Year == year)
                    .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
                foreach (var app in listApp)
                {
                    var appData = new AppDataViewModel()
                    {
                        Datasets = new List<decimal>(),
                        ServiceApplicationId = (Guid) app.ServiceApplicationId,
                        ServiceApplicationName = app.ServiceApplicationName
                    };
                    for (var i = 1; i <= 12; i++)
                    {
                        var month = i;
                        var commission = await _unitOfWork.ServiceOrderRepository
                            .Get(s =>
                                s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                                s.CreateDate.Value.Month == month &&
                                s.CreateDate.Value.Year == year)
                            .SumAsync(o => o.SystemCommission);
                        appData.Datasets.Add((decimal) commission);
                    }

                    result.Datas.Add(appData);
                }
            }

            return result;
        }

        public async Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionSystemByDayOfMonth(int month,
            int year, List<Guid> serviceApplicationIds)

        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);
            var result = new ServiceOrderCommissionLineChartViewModel()
            {
                Datas = new List<AppDataViewModel>()
            };
            // Get by list service app ID
            if (serviceApplicationIds.Count != 0)
                foreach (var serviceApplicationId in serviceApplicationIds)
                {
                    if (serviceApplicationId == null)
                    {
                        _logger.LogInformation("App id can not null.");
                        throw new ErrorResponse((int) HttpStatusCode.BadRequest, "App id can not null.");
                    }

                    var appOrder = await _unitOfWork.ServiceOrderRepository.Get(s =>
                            s.CreateDate.Value.Month == month &&
                            s.CreateDate.Value.Year == year &&
                            s.ServiceApplicationId == serviceApplicationId)
                        .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (appOrder == null)
                    {
                        var name = await _serviceApplicationService.GetNameById(serviceApplicationId);
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = name
                        };
                        result.Datas.Add(appData);
                    }
                    else
                    {
                        var appData = new AppDataViewModel()
                        {
                            Datasets = new List<decimal>(),
                            ServiceApplicationId = serviceApplicationId,
                            ServiceApplicationName = appOrder.ServiceApplicationName
                        };
                        for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                        {
                            var commission = await _unitOfWork.ServiceOrderRepository
                                .Get(s =>
                                    s.ServiceApplicationId.Equals(appOrder.ServiceApplicationId) &&
                                    s.CreateDate.Value.Day == dt.Day &&
                                    s.CreateDate.Value.Month == month &&
                                    s.CreateDate.Value.Year == year)
                                .SumAsync(o => o.SystemCommission);
                            appData.Datasets.Add((decimal) commission);
                        }

                        result.Datas.Add(appData);
                    }
                }
            // Get all
            else
            {
                var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                        s.CreateDate.Value.Month == month &&
                        s.CreateDate.Value.Year == year)
                    .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                listApp = listApp.GroupBy(o => o.ServiceApplicationId).Select(g => g.First()).ToList();
                foreach (var app in listApp)
                {
                    var appData = new AppDataViewModel()
                    {
                        Datasets = new List<decimal>(),
                        ServiceApplicationId = (Guid) app.ServiceApplicationId,
                        ServiceApplicationName = app.ServiceApplicationName
                    };
                    for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                    {
                        var commission = await _unitOfWork.ServiceOrderRepository
                            .Get(s =>
                                s.ServiceApplicationId.Equals(app.ServiceApplicationId) &&
                                s.CreateDate.Value.Day == dt.Day &&
                                s.CreateDate.Value.Month == month &&
                                s.CreateDate.Value.Year == year)
                            .SumAsync(o => o.SystemCommission);
                        appData.Datasets.Add((decimal) commission);
                    }

                    result.Datas.Add(appData);
                }
            }

            return result;
        }
    }
}