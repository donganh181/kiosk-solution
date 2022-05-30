using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class TemplateService : ITemplateService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ITemplateService> _logger;

        public TemplateService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ITemplateService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
    }
}