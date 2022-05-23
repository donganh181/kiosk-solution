using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace kiosk_solution.Business.Services.impl
{
    public class KioskService : IKioskService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IKioskService> _logger;

        public KioskService(IConfigurationProvider mapper, IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<IKioskService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<KioskViewModel> UpdateStatus(Guid id)
        {
            var kiosk = await _unitOfWork.KioskRepository.Get(k => k.Id.Equals(id)).FirstOrDefaultAsync();
            if (kiosk == null)
            {
                _logger.LogInformation("Kiosk not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Kiosk not found.");
            }
            if (kiosk.Status.Equals(StatusConstants.ACTIVE))
                kiosk.Status = StatusConstants.DEACTIVATE;
            else
                kiosk.Status = StatusConstants.ACTIVE;
            try
            {
                _unitOfWork.KioskRepository.Update(kiosk);
                await _unitOfWork.SaveAsync();
                var result = _mapper.CreateMapper().Map<KioskViewModel>(kiosk);
                return result;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }
    }
}