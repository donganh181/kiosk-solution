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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services.impl
{
    public class ServiceApplicationPublishRequestService : IServiceApplicationPublishRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IServiceApplicationPublishRequestService> _logger;
        private readonly IServiceApplicationService _appService;

        public ServiceApplicationPublishRequestService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<ServiceApplicationPublishRequestService> logger, IServiceApplicationService appService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _appService = appService;
        }

        public async Task<ServiceApplicationPublishRequestViewModel> Create(Guid creatorId,
            ServiceApplicationPublishRequestCreateViewModel model)
        {
            var app = await _appService.GetById(Guid.Parse(model.ServiceApplicationId + ""));
            if (app == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            if (!app.PartyId.Equals(creatorId))
            {
                _logger.LogInformation("Cannot publish other user's application.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Cannot publish other user's application.");
            }

            if (app.Status.Equals(StatusConstants.AVAILABLE) || app.Status.Equals(StatusConstants.PENDING))
            {
                _logger.LogInformation("App did not meet requirement to publish.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "App did not meet requirement to publish.");
            }

            var request = _mapper.Map<ServiceApplicationPublishRequest>(model);
            request.CreatorId = creatorId;
            request.Status = StatusConstants.IN_PROGRESS;
            request.CreateDate = DateTime.Now;
            try
            {
                await _unitOfWork.ServiceApplicationPublishRequestRepository.InsertAsync(request);
                await _unitOfWork.SaveAsync();

                bool check = await _appService.SetStatus(app.Id, StatusConstants.PENDING);
                if (check == false)
                {
                    _logger.LogInformation("Server Error.");
                    throw new ErrorResponse((int) HttpStatusCode.InternalServerError, "Server Error.");
                }

                var result = await _unitOfWork.ServiceApplicationPublishRequestRepository
                    .Get(r => r.Id.Equals(request.Id))
                    .Include(r => r.Creator)
                    .Include(r => r.ServiceApplication)
                    .ProjectTo<ServiceApplicationPublishRequestViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<ServiceApplicationPublishRequestSearchViewModel>> GetAllWithPaging(
            string role, Guid id, ServiceApplicationPublishRequestSearchViewModel model, int size, int pageNum)
        {
            object requests = null;

            if (role.Equals(RoleConstants.ADMIN))
            {
                requests = _unitOfWork.ServiceApplicationPublishRequestRepository
                    .Get()
                    .Include(r => r.Creator)
                    .Include(r => r.Handler)
                    .Include(r => r.ServiceApplication)
                    .ProjectTo<ServiceApplicationPublishRequestSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(r => r.CreateDate);
            }


            if (role.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                requests = _unitOfWork.ServiceApplicationPublishRequestRepository
                    .Get(r => r.CreatorId.Equals(id))
                    .Include(r => r.Creator)
                    .Include(r => r.Handler)
                    .Include(r => r.ServiceApplication)
                    .ProjectTo<ServiceApplicationPublishRequestSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(r => r.CreateDate);
            }

            if (requests == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var listRequest = (IQueryable<ServiceApplicationPublishRequestSearchViewModel>) requests;

            var listPaging = listRequest
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<ServiceApplicationPublishRequestSearchViewModel>
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

        public async Task<ServiceApplicationPublishRequestViewModel> GetById(Guid partyId, string role, Guid requestId)
        {
            ServiceApplicationPublishRequestViewModel publishRequest = null;
            if (role.Equals(RoleConstants.ADMIN))
            {
                publishRequest = await _unitOfWork.ServiceApplicationPublishRequestRepository
                    .Get(p => p.Id.Equals(requestId))
                    .Include(r => r.Creator)
                    .Include(r => r.Handler)
                    .Include(r => r.ServiceApplication)
                    .ProjectTo<ServiceApplicationPublishRequestViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            }
            else if (role.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                publishRequest = await _unitOfWork.ServiceApplicationPublishRequestRepository
                    .Get(p => p.Id.Equals(requestId) && p.CreatorId.Equals(partyId))
                    .Include(r => r.Creator)
                    .Include(r => r.Handler)
                    .Include(r => r.ServiceApplication)
                    .ProjectTo<ServiceApplicationPublishRequestViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            }
            else
            {
                _logger.LogInformation("Server Error");
                throw new ErrorResponse((int) HttpStatusCode.InternalServerError, "Server Error.");
            }

            if (publishRequest == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            return publishRequest;
        }

        public async Task<ServiceApplicationPublishRequestViewModel> Update(Guid handlerId,
            UpdateServiceApplicationPublishRequestViewModel model)
        {
            var publishRequest = await _unitOfWork.ServiceApplicationPublishRequestRepository
                .Get(p => p.Id.Equals(model.Id)).FirstOrDefaultAsync();
            if (publishRequest == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            if (!publishRequest.Status.Equals(StatusConstants.IN_PROGRESS))
            {
                _logger.LogInformation("Invalid status to use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid status to use this feature.");
            }

            publishRequest.HandlerId = handlerId;
            publishRequest.HandlerComment = model.HandlerComment;
            publishRequest.Status = model.Status;
            try
            {
                _unitOfWork.ServiceApplicationPublishRequestRepository.Update(publishRequest);
                await _unitOfWork.SaveAsync();
                if (publishRequest.Status.Equals(StatusConstants.APPROVED))
                {
                    await _appService.SetStatus(Guid.Parse(publishRequest.ServiceApplicationId + ""),
                        StatusConstants.AVAILABLE);
                }
                else if (publishRequest.Status.Equals(StatusConstants.DENIED))
                {
                    await _appService.SetStatus(Guid.Parse(publishRequest.ServiceApplicationId + ""),
                        StatusConstants.UNAVAILABLE);
                }
                else
                {
                    _logger.LogInformation("Server Error.");
                    throw new ErrorResponse((int) HttpStatusCode.InternalServerError, "Server Error.");
                }

                var result = await _unitOfWork.ServiceApplicationPublishRequestRepository
                    .Get(r => r.Id.Equals(publishRequest.Id))
                    .Include(r => r.Creator)
                    .Include(r => r.Handler)
                    .Include(r => r.ServiceApplication)
                    .ProjectTo<ServiceApplicationPublishRequestViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<ServiceApplicationPublishRequestViewModel> GetInprogressByAppId(Guid appId)
        {
            var publishRequest = await _unitOfWork.ServiceApplicationPublishRequestRepository
                .Get(p => p.ServiceApplicationId.Equals(appId) && p.Status.Equals(StatusConstants.IN_PROGRESS))
                .Include(r => r.Creator)
                .Include(r => r.Handler)
                .Include(r => r.ServiceApplication)
                .ProjectTo<ServiceApplicationPublishRequestViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (publishRequest == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            return publishRequest;
        }
    }
}