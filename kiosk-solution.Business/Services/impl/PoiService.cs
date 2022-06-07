using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
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

        public async Task<PoiViewModel> Create(Guid partyId, string roleName, PoiCreateViewModel model)
        {
            var poi = _mapper.Map<Poi>(model);
            poi.OpenTime = TimeSpan.Parse(model.StringOpenTime);
            poi.CreateDate = DateTime.Now;
            poi.CreatorId = partyId;
            poi.Status = StatusConstants.ACTIVE;
            if (roleName.Equals(RoleConstants.ADMIN))
                poi.Type = TypeConstants.CREATE_BY_ADMIN;
            else
                poi.Type = TypeConstants.CREATE_BY_LOCATION_OWNER;
            try
            {
                await _unitOfWork.PoiRepository.InsertAsync(poi);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<PoiViewModel>(poi);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity,"Invalid data.");
            }
        }
    }
}