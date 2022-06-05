﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
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
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Business.Services.impl
{
    public class ServiceApplicationService : IServiceApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IServiceApplicationService> _logger;
        private readonly IFileService _fileService;

        public ServiceApplicationService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<ServiceApplicationService> logger, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<DynamicModelResponse<ServiceApplicationSearchViewModel>> GetAllWithPaging(string role,
            Guid id, ServiceApplicationSearchViewModel model, int size, int pageNum)
        {
            object apps = null;
            if (role.Equals(RoleConstants.ADMIN))
            {
                apps = _unitOfWork.ServiceApplicationRepository
                    .Get()
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .ProjectTo<ServiceApplicationSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(a => a.Name);
            
            }
            if (role.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                apps = _unitOfWork.ServiceApplicationRepository
                    .Get(a => a.PartyId.Equals(id))
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .ProjectTo<ServiceApplicationSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(a => a.Name);
            }

            if(apps == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }
            var listApp = (IQueryable<ServiceApplicationSearchViewModel>)apps;

            var listPaging =
                listApp.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<ServiceApplicationSearchViewModel>
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

        public async Task<ServiceApplicationViewModel> UpdateInformation(Guid updaterId,
            UpdateServiceApplicationViewModel model)
        {
            var app = await _unitOfWork.ServiceApplicationRepository
                .Get(a => a.Id.Equals(model.Id))
                .Include(a => a.AppCategory)
                .FirstOrDefaultAsync();
            if (!app.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("User not match.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot use this feature.");
            }

            if (app == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found.");
            }

            app.Name = model.Name;
            app.Description = model.Description;
            app.Link = model.Link;
            app.AppCategoryId = model.AppCategoryId;

            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();

                var newLogo =
                    await _fileService.UploadImageToFirebase(model.Logo, app.AppCategory.Name, model.Id, "Logo");
                app.Logo = newLogo;
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceApplicationViewModel>(app);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<ServiceApplicationViewModel> Create(Guid partyId, CreateServiceApplicationViewModel model)
        {
            var serviceApplication = _mapper.Map<ServiceApplication>(model);
            serviceApplication.PartyId = partyId;
            serviceApplication.Status = StatusConstants.INCOMPLETE;
            try
            {
                await _unitOfWork.ServiceApplicationRepository.InsertAsync(serviceApplication);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceApplicationViewModel>(serviceApplication);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<ServiceApplicationViewModel> UpdateLogo(Guid partyId, UpdateLogoViewModel model)
        {
            var serviceApplication = await _unitOfWork.ServiceApplicationRepository
                .Get(a => a.Id.Equals(model.ServiceApplicationId))
                .Include(a => a.AppCategory).FirstOrDefaultAsync();
            if (serviceApplication == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            if (!serviceApplication.PartyId.Equals(partyId))
            {
                _logger.LogInformation("User not match.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot use this feature.");
            }

            try
            {
                var logo = await _fileService.UploadImageToFirebase(model.Logo, serviceApplication.AppCategory.Name,
                    model.ServiceApplicationId, "Logo");
                serviceApplication.Logo = logo;
                serviceApplication.Status = StatusConstants.UNAVAILABLE;
                _unitOfWork.ServiceApplicationRepository.Update(serviceApplication);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceApplicationViewModel>(serviceApplication);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<ServiceApplicationViewModel> GetById(Guid id)
        {
            return await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(id)).ProjectTo<ServiceApplicationViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<bool> SetStatus(Guid id, string status)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            if(app == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }
            app.Status = status;

            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }
    }
}