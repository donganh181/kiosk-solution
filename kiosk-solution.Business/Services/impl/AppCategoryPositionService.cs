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
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            if(!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(model.AppCategoryId + "")))
            {
                _logger.LogInformation($"{partyId} has no app in this category.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Your account has no app in this category.");
            }

            var position = _mapper.Map<AppCategoryPosition>(model);

            try
            {
                await _unitOfWork.AppCategoryPositionRepository.InsertAsync(position);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.Id.Equals(position.Id))
                    .Include(p => p.Template)
                    .Include(p => p.AppCategory)
                    .ProjectTo<AppCategoryPositionViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
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
