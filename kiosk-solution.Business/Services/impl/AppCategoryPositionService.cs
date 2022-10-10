using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class AppCategoryPositionService : IAppCategoryPositionService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IAppCategoryPositionService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITemplateService _templateService;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;

        public AppCategoryPositionService(IMapper mapper, ILogger<IAppCategoryPositionService> logger,
            IUnitOfWork unitOfWork, IPartyServiceApplicationService partyServiceApplicationService,
            ITemplateService templateService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _partyServiceApplicationService = partyServiceApplicationService;
            _templateService = templateService;
        }

        public async Task<AppCategoryPositionViewModel> Create(Guid partyId, AppCategoryPositionCreateViewModel model)
        {
            //check if template is deleted
            var template = await _templateService.GetById(Guid.Parse(model.TemplateId + ""));
            if (template.Status.Equals(StatusConstants.DELETED))
            {
                _logger.LogInformation("Template is deleted.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Template is deleted.");
            }

            //check if there are 2 or more cate are in the same position
            if (model.ListPosition.GroupBy(x => new {x.RowIndex, x.ColumnIndex}).Where(x => x.Count() > 1)
                .FirstOrDefault() != null)
            {
                _logger.LogInformation("There are 2 or more cate are in the same position.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "There are 2 or more cate are in the same position.");
            }

            //check if template owner
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            //case this template has already set cate on it
            if (await _unitOfWork.AppCategoryPositionRepository.Get(p => p.TemplateId.Equals(model.TemplateId))
                .FirstOrDefaultAsync() != null)
            {
                _logger.LogInformation($"{model.TemplateId} has already set cate on it. Please use Update function.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{model.TemplateId} has already set cate on it. Please use Update function.");
            }

            //check if location owner did not have any app in cate
            foreach (var cate in model.ListPosition)
            {
                if (!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(cate.AppCategoryId + "")))
                {
                    _logger.LogInformation($"{partyId} has no app in this category.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                        "Your account has no app in this category.");
                }
            }

            try
            {
                foreach (var pos in model.ListPosition)
                {
                    var position = _mapper.Map<AppCategoryPosition>(pos);
                    position.TemplateId = model.TemplateId;

                    await _unitOfWork.AppCategoryPositionRepository.InsertAsync(position);
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

                var listPos = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.TemplateId.Equals(model.TemplateId))
                    .Include(p => p.Template)
                    .Include(p => p.AppCategory)
                    .ProjectTo<CategoryPositionDetailViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var result = new AppCategoryPositionViewModel(model.TemplateId, template.Name, listPos);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<AppCategoryPositionViewModel> Update(Guid partyId, AppCategoryPositionUpdateViewModel model)
        {
            //check if there are 2 or more cate are in the same position
            if (model.ListPosition.GroupBy(x => new {x.RowIndex, x.ColumnIndex}).Where(x => x.Count() > 1)
                .FirstOrDefault() != null)
            {
                _logger.LogInformation("There are 2 or more cate are in the same position.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "There are 2 or more cate are in the same position.");
            }

            //check if template owner
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            //check if location owner did not have any app in cate
            foreach (var cate in model.ListPosition)
            {
                if (!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(cate.AppCategoryId + "")))
                {
                    _logger.LogInformation($"{partyId} has no app in this category.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                        "Your account has no app in this category.");
                }
            }

            try
            {
                var listDelete = await _unitOfWork.AppCategoryPositionRepository
                    .Get(e => e.TemplateId.Equals(model.TemplateId)).ToListAsync();
                foreach (var pos in listDelete)
                    _unitOfWork.AppCategoryPositionRepository.Delete(pos);
                await _unitOfWork.SaveAsync();

                foreach (var pos in model.ListPosition)
                {
                    var position = _mapper.Map<AppCategoryPosition>(pos);
                    position.TemplateId = model.TemplateId;

                    await _unitOfWork.AppCategoryPositionRepository.InsertAsync(position);
                }

                var template = await _unitOfWork.TemplateRepository.Get(t => t.Id.Equals(model.TemplateId))
                    .FirstOrDefaultAsync();
                await _unitOfWork.SaveAsync();
                var listPos = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.TemplateId.Equals(model.TemplateId))
                    .Include(p => p.Template)
                    .Include(p => p.AppCategory)
                    .ProjectTo<CategoryPositionDetailViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var result = new AppCategoryPositionViewModel(model.TemplateId, template.Name, listPos);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<AppCategoryPositionGetViewModel> GetById(Guid partyId, Guid templateId)
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

            var listPosition = new List<AppCategoryPositionByRowViewModel>();
            var components = new List<AppCategoryPositionDetailViewModel>();
            int rowIndex = -1;
            do
            {
                rowIndex++;
                components = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.TemplateId.Equals(templateId) && p.RowIndex == rowIndex)
                    .ProjectTo<AppCategoryPositionDetailViewModel>(_mapper.ConfigurationProvider).AsQueryable()
                    .OrderBy(p => p.ColumnIndex)
                    .ToListAsync();
                if (components.Count == 0)
                    break;
                listPosition.Add(new AppCategoryPositionByRowViewModel(rowIndex, components));
            } while (true);

            if (listPosition == null)
            {
                _logger.LogInformation("Can not found positions.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found positions.");
            }

            var result = new AppCategoryPositionGetViewModel(templateId, template.Name, listPosition);
            return result;
        }
    }
}