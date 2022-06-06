using System;
using System.Threading.Tasks;
using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class PoiService : IPoiService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPoiService> _logger;

        public PoiService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IPoiService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PoiViewModel> Create(Guid partyId, PoiCreateViewModel model)
        {
            var poi = _mapper.Map<Poi>(model);
            poi.CreateDate = DateTime.Today;
            poi.CreatorId = partyId;

            try
            {
                var result = _mapper.Map<PoiViewModel>(poi);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}