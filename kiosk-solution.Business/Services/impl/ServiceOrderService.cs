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
            var serviceOrder = _mapper.Map<ServiceOrder>(model);
            dynamic order = JObject.Parse(model.OrderDetail);
            serviceOrder.Total = 0;
            foreach (var item in order.items)
            {
                serviceOrder.Total += Decimal.Parse(item.Price.ToString());
            }

            var appCate = await _serviceApplicationService.GetCommissionById(model.ServiceApplicationId);
            serviceOrder.Commission =
                serviceOrder.Total * Decimal.Parse((appCate.CommissionPercentage - 1).ToString()) / 100;
            serviceOrder.SystemCommission = serviceOrder.Total * 1 / 100;
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

        public async Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommission(Guid partyId, Guid kioskId)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s => s.KioskId.Equals(kioskId))
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.serviceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.KioskId.Equals(kioskId) && s.ServiceApplicationId.Equals(app.serviceApplicationId))
                    .SumAsync(o => o.Commission);
                app.totalCommission = (double) commission;
            }

            return listApp;
        }

        public async Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommissionByMonth(Guid partyId,
            Guid kioskId,
            int month, int year)
        {
            var kiosk = await _kioskService.GetByIdWithParyId(kioskId, partyId);
            if (kiosk == null)
            {
                _logger.LogInformation("You can not use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not use this feature.");
            }

            var listApp = await _unitOfWork.ServiceOrderRepository.Get(s =>
                    s.KioskId.Equals(kioskId) &&
                    s.CreateDate.Value.Month == month &&
                    s.CreateDate.Value.Year == year)
                .ProjectTo<ServiceOrderCommissionSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            listApp = listApp.GroupBy(o => o.serviceApplicationId).Select(g => g.First()).ToList();
            foreach (var app in listApp)
            {
                var commission = await _unitOfWork.ServiceOrderRepository
                    .Get(s => s.KioskId.Equals(kioskId) &&
                              s.ServiceApplicationId.Equals(app.serviceApplicationId) &&
                              s.CreateDate.Value.Month == month &&
                              s.CreateDate.Value.Year == year)
                    .SumAsync(o => o.Commission);
                app.totalCommission = (double) commission;
            }

            return listApp;
        }
    }
}