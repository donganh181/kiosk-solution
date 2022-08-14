using System;
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

namespace kiosk_solution.Business.Services.impl
{
    public class TemplateService : ITemplateService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ITemplateService> _logger;

        public TemplateService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ITemplateService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TemplateViewModel> Create(Guid id, TemplateCreateViewModel model)
        {
            var template = _mapper.Map<Template>(model);
            template.PartyId = id;
            template.CreateDate = DateTime.Now;
            template.Status = StatusConstants.INCOMPLETE;
            try
            {
                await _unitOfWork.TemplateRepository.InsertAsync(template);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<TemplateViewModel>(template);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<TemplateSearchViewModel>> GetAllWithPaging(Guid id, TemplateSearchViewModel model, int size, int pageNum)
        {
            var templates = _unitOfWork.TemplateRepository
                .Get(t => t.PartyId.Equals(id) && !t.Status.Equals(StatusConstants.DELETED))
                .ProjectTo<TemplateSearchViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter(model)
                .AsQueryable().OrderByDescending(t => t.Name);

            var listPaging =
                templates.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<TemplateSearchViewModel>
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

        public async Task<bool> IsOwner(Guid partyId, Guid templateId)
        {
            var template = await _unitOfWork.TemplateRepository.Get(s => s.Id.Equals(templateId)).FirstOrDefaultAsync();
            if (template == null)
            {
                _logger.LogInformation($"Template {templateId} is not exist.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Template is not exist.");
            }
            bool result = template.PartyId.Equals(partyId);
            return result;
        }

        public async Task<TemplateViewModel> UpdateInformation(Guid updaterId, TemplateUpdateViewModel model)
        {
            var template = await _unitOfWork.TemplateRepository
                .Get(t => t.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (!template.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("Your account cannot update template of other account.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot update template of other account.");
            }

            template.Name = model.Name;
            template.Description = model.Description;

            try
            {
                _unitOfWork.TemplateRepository.Update(template);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<TemplateViewModel>(template);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<TemplateViewModel> UpdateStatus(Guid updaterId, Guid templateId)
        {
            var template = await _unitOfWork.TemplateRepository
                .Get(t => t.Id.Equals(templateId))
                .FirstOrDefaultAsync();

            if (!template.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("Your account cannot update template of other account.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot update template of other account.");
            }
            template.Status = StatusConstants.DELETED;
            try
            {
                _unitOfWork.TemplateRepository.Update(template);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<TemplateViewModel>(template);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<TemplateViewModel> GetById(Guid templateId)
        {
            var template = await _unitOfWork.TemplateRepository.Get(t => t.Id.Equals(templateId)).FirstOrDefaultAsync();
            if(template == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
            }
            if (template.Status.Equals(StatusConstants.DELETED))
            {
                _logger.LogInformation("Template Deleted.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Template Deleted.");
            }
            var result = _mapper.Map<TemplateViewModel>(template);
            return result;
        }

        public async Task<bool> UpdateStatusToComplete(Guid updaterId, Guid templateId)
        {
            var template = await _unitOfWork.TemplateRepository
                .Get(t => t.Id.Equals(templateId))
                .FirstOrDefaultAsync();

            if (!template.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("Your account cannot update template of other account.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot update template of other account.");
            }
            template.Status = StatusConstants.COMPLETE;
            try
            {
                _unitOfWork.TemplateRepository.Update(template);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<TemplateViewModel>(template);
                return true;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<TemplateDetailViewModel> GetDetailById(Guid templateId)
        {
            var template = await _unitOfWork.TemplateRepository
                .Get(t => t.Id.Equals(templateId))
                .Include(x => x.AppCategoryPositions)
                .Include(x => x.EventPositions)
                .ProjectTo<TemplateDetailViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();


            if (template == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
            }
            if (template.Status.Equals(StatusConstants.DELETED))
            {
                _logger.LogInformation("Template Deleted.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Template Deleted.");
            }

            return template;
        }
    }
}