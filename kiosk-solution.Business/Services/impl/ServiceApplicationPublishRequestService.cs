using AutoMapper;
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
    public class ServiceApplicationPublishRequestService : IServiceApplicationPublishRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceApplicationPublishRequestService> _logger;
        private readonly IServiceApplicationService _appService;

        public ServiceApplicationPublishRequestService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<ServiceApplicationPublishRequestService> logger, IServiceApplicationService appService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _appService = appService;
        }

        public async Task<ServiceApplicationPublishRequestViewModel> Create(Guid creatorId, ServiceApplicationPublishRequestCreateViewModel model)
        {
           
            var app = await _appService.GetById(Guid.Parse(model.ServiceApplicationId + ""));
            if (app == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            }

            if (!app.PartyId.Equals(creatorId))
            {
                _logger.LogInformation("Cannot publish other user's application.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Cannot publish other user's application.");
            }

            if (app.Status.Equals(StatusConstants.INCOMPLETE) || app.Status.Equals(StatusConstants.AVAILABLE))
            {
                _logger.LogInformation("App did not meet requirement to publish.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "App did not meet requirement to publish.");
            }
            var listRequest = await _unitOfWork.ServiceApplicationPublishRequestRepository.Get(r => r.ServiceApplicationId.Equals(app.Id)).ToListAsync();
            if (listRequest.Count != 0)
            {
                foreach(var checkRequest in listRequest)
                {
                    if (checkRequest.Status.Equals(StatusConstants.IN_PROGRESS))
                    {
                        _logger.LogInformation("You have already request this application.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You have already request this application.");
                    }
                }
            }
            var request = _mapper.Map<ServiceApplicationPublishRequest>(model);
            request.Status = StatusConstants.IN_PROGRESS;
            try
            {
                await _unitOfWork.ServiceApplicationPublishRequestRepository.InsertAsync(request);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceApplicationPublishRequestViewModel>(request);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<ServiceApplicationPublishRequestViewModel> Update(Guid handlerId, UpdateServiceApplicationPublishRequestViewModel model)
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
                var result = _mapper.Map<ServiceApplicationPublishRequestViewModel>(publishRequest);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }

        }
    }
}
