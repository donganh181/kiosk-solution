using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            //check if there are 2 or more cate are in the same position
            if(model.ListPosition.GroupBy(x => new {x.RowIndex, x.ColumnIndex}).Where(x => x.Count() > 1).FirstOrDefault() != null)
            {
                _logger.LogInformation("There are 2 or more cate are in the same position.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "There are 2 or more cate are in the same position.");
            }
            //check if template owner
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            //case this template has already set cate on it
            if(await _unitOfWork.AppCategoryPositionRepository.Get(p => p.TemplateId.Equals(model.TemplateId)).FirstOrDefaultAsync() != null)
            {
                _logger.LogInformation($"{model.TemplateId} has already set cate on it. Please use Update function.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{model.TemplateId} has already set cate on it. Please use Update function.");
            }
            //check if location owner did not have any app in cate
            foreach(var cate in model.ListPosition)
            {
                if (!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(cate.AppCategoryId + "")))
                {
                    _logger.LogInformation($"{partyId} has no app in this category.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Your account has no app in this category.");
                }
            }
            try
            {
                foreach(var pos in model.ListPosition)
                {
                    var position = _mapper.Map<AppCategoryPosition>(pos);
                    position.TemplateId = model.TemplateId;

                    await _unitOfWork.AppCategoryPositionRepository.InsertAsync(position);
                }
                await _unitOfWork.SaveAsync();
                var listPos = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.TemplateId.Equals(model.TemplateId))
                    .Include(p => p.Template)
                    .Include(p => p.AppCategory)
                    .ProjectTo<CategoryPositionDetailViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var result = new AppCategoryPositionViewModel()
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

        public async Task<AppCategoryPositionViewModel> Update(Guid partyId, AppCategoryPositionUpdateViewModel model)
        {
            AppCategoryPosition catePos = null;
            List<AppCategoryPosition> listCheck = new List<AppCategoryPosition>();
            //check if there are 2 or more cate are in the same position
            if (model.ListPosition.GroupBy(x => new { x.RowIndex, x.ColumnIndex }).Where(x => x.Count() > 1).FirstOrDefault() != null)
            {
                _logger.LogInformation("There are 2 or more cate are in the same position.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "There are 2 or more cate are in the same position.");
            }
            //check if template owner
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            //check if location owner did not have any app in cate
            foreach (var cate in model.ListPosition)
            {
                if (!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(cate.AppCategoryId + "")))
                {
                    _logger.LogInformation($"{partyId} has no app in this category.");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Your account has no app in this category.");
                }
            }
            try
            {
                foreach(var pos in model.ListPosition)
                {
                    catePos = await _unitOfWork.AppCategoryPositionRepository.Get(p => p.Id.Equals(pos.Id)).FirstOrDefaultAsync();

                    catePos.AppCategoryId = pos.AppCategoryId;
                    catePos.RowIndex = pos.RowIndex;
                    catePos.ColumnIndex = pos.ColumnIndex;
                    listCheck.Add(catePos);
                    _unitOfWork.AppCategoryPositionRepository.Update(catePos);
                }
                await _unitOfWork.SaveAsync();

                var listPos = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.TemplateId.Equals(model.TemplateId))
                    .Include(p => p.Template)
                    .Include(p => p.AppCategory)
                    .ProjectTo<CategoryPositionDetailViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                var result = new AppCategoryPositionViewModel()
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
    }
}
