﻿using System;
using System.Linq;
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

        public ServiceOrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IServiceOrderService> logger,IServiceApplicationService serviceApplicationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _serviceApplicationService = serviceApplicationService;
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
            serviceOrder.Commission = serviceOrder.Total * Decimal.Parse((appCate.CommissionPercentage-1).ToString()) / 100;
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
    }
}