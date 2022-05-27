﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services.impl
{
    public class KioskLocationService : IKioskLocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationProvider _mapper;
        private readonly ILogger<IKioskLocationService> _logger;

        public KioskLocationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IKioskLocationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper.ConfigurationProvider;
            _logger = logger;
        }

        public async Task<KioskLocationViewModel> CreateNew(CreateKioskLocationViewModel model)
        {
            var kioskLocation = _mapper.CreateMapper().Map<KioskLocation>(model);
            kioskLocation.CreateDate = DateTime.Now;
            kioskLocation.Status = StatusConstants.ACTIVE;
            
            try
            {
                await _unitOfWork.KioskLocationRepository.InsertAsync(kioskLocation);
                await _unitOfWork.SaveAsync();

                var result = _mapper.CreateMapper().Map<KioskLocationViewModel>(kioskLocation);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<KioskLocationSearchViewModel>> GetAllWithPaging(KioskLocationSearchViewModel model, int size, int pageNum)
        {
            var kioskLocations = _unitOfWork.KioskLocationRepository.Get().ProjectTo<KioskLocationSearchViewModel>(_mapper)
                .DynamicFilter(model)
                .AsQueryable().OrderByDescending(l => l.Name);

            var listPaging = kioskLocations
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if(listPaging.Item2.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<KioskLocationSearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Item1
                },
                Data = listPaging.Item2.ToList()
            };
            return result;
        }
    }
}