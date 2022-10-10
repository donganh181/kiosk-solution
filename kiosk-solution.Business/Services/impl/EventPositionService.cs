using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class EventPositionService : IEventPositionService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITemplateService _templateService;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;
        private readonly IEventService _eventService;
        
        public EventPositionService(IMapper mapper, ILogger<IEventPositionService> logger, 
            IUnitOfWork unitOfWork, IPartyServiceApplicationService partyServiceApplicationService,
            ITemplateService templateService, IEventService eventService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _partyServiceApplicationService = partyServiceApplicationService;
            _templateService = templateService;
            _eventService = eventService;
        }
        
        public async Task<EventPositionViewModel> Create(Guid partyId, EventPositionCreateViewModel model)
        {
            //check if template is deleted
            var template = await _templateService.GetById(Guid.Parse(model.TemplateId + ""));
            if (template.Status.Equals(StatusConstants.DELETED))
            {
                _logger.LogInformation("Template is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Template is deleted.");
            }

            //check if there are 2 or more event are in the same position
            if (model.ListPosition.GroupBy(x => new {x.RowIndex, x.ColumnIndex}).Where(x => x.Count() > 1).FirstOrDefault() != null)
            {
                _logger.LogInformation("There are 2 or more event are in the same position.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "There are 2 or more event are in the same position.");
            }
            //check if template owner
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            //case this template has already set event on it
            if(await _unitOfWork.EventPositionRepository.Get(p => p.TemplateId.Equals(model.TemplateId)).FirstOrDefaultAsync() != null)
            {
                _logger.LogInformation($"{model.TemplateId} has already set event on it. Please use Update function.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{model.TemplateId} has already set event on it. Please use Update function.");
            }
            try
            {
                foreach (var pos in model.ListPosition)
                {
                    var position = _mapper.Map<EventPosition>(pos);
                    position.TemplateId = model.TemplateId;
                    if (position.EventId == null)
                    {
                        _logger.LogInformation($"Event id is required.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Event id is required.");
                    }
                    //check if event is deleted or not
                    var checkEvent = await _eventService.GetById(Guid.Parse(position.EventId + ""));

                    await _unitOfWork.EventPositionRepository.InsertAsync(position);
                }
                await _unitOfWork.SaveAsync();

                if (!template.Status.Equals(StatusConstants.COMPLETE))
                {
                    var check = await _templateService.UpdateStatusToComplete(partyId, template.Id);
                    if (!check)
                    {
                        _logger.LogInformation("Server error.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server error.");
                    }
                }

                var listPos = await _unitOfWork.EventPositionRepository
                    .Get(p => p.TemplateId.Equals(model.TemplateId))
                    .Include(p => p.Template)
                    .Include(p => p.Event)
                    .ProjectTo<EventPositionDetailViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var result = new EventPositionViewModel()
                {
                    TemplateId = model.TemplateId,
                    TemplateName = listPos.FirstOrDefault().TemplateName,
                    ListPosition = listPos
                };
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<EventPositionViewModel> Update(Guid partyId, EventPositionUpdateViewModel model)
        {
            //check if there are 2 or more event are in the same position
            if (model.ListPosition.GroupBy(x => new { x.RowIndex, x.ColumnIndex }).Where(x => x.Count() > 1).FirstOrDefault() != null)
            {
                _logger.LogInformation("There are 2 or more event are in the same position.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "There are 2 or more event are in the same position.");
            }
            //check if template owner
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            try
            {
                var listDelete = await _unitOfWork.EventPositionRepository
                    .Get(e => e.TemplateId.Equals(model.TemplateId)).ToListAsync();
                foreach (var pos in listDelete)
                    _unitOfWork.EventPositionRepository.Delete(pos);
                await _unitOfWork.SaveAsync();
                
                foreach(var pos in model.ListPosition)
                {
                    var position = _mapper.Map<EventPosition>(pos);
                    position.TemplateId = model.TemplateId;

                    await _unitOfWork.EventPositionRepository.InsertAsync(position);
                }
                await _unitOfWork.SaveAsync();
                
                var listPos = await _unitOfWork.EventPositionRepository
                    .Get(p => p.TemplateId.Equals(model.TemplateId))
                    .Include(p => p.Template)
                    .Include(p => p.Event)
                    .ProjectTo<EventPositionDetailViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var result = new EventPositionViewModel()
                {
                    TemplateId = model.TemplateId,
                    TemplateName = listPos.FirstOrDefault().TemplateName,
                    ListPosition = listPos
                };
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<EventPositionGetViewModel> GetById(Guid partyId, Guid templateId)
        {
            var template = await _templateService.GetById(templateId);
            if (template == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            if (!template.PartyId.Equals(partyId))
            {
                _logger.LogInformation("You cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot use this feature.");
            }

            var listPosition = new List<EventPositionByRowViewModel>();
            var components = new List<EventPositionDetailGetViewModel>();
            int rowIndex = -1;
            do
            {
                rowIndex++;
                components = await _unitOfWork.EventPositionRepository
                    .Get(p => p.TemplateId.Equals(templateId) && p.RowIndex == rowIndex)
                    .ProjectTo<EventPositionDetailGetViewModel>(_mapper.ConfigurationProvider).AsQueryable()
                    .OrderBy(p => p.ColumnIndex)
                    .ToListAsync();
                listPosition.Add(new EventPositionByRowViewModel(rowIndex, components));
            } while (rowIndex < 2);

            if (listPosition == null)
            {
                _logger.LogInformation("Can not found positions.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found positions.");
            }

            var result = new EventPositionGetViewModel(templateId, template.Name, listPosition);
            return result;
        }
    }
}