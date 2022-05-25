using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace kiosk_solution.Business.Services.impl
{
    public class KioskService : IKioskService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IKioskService> _logger;

        public KioskService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IKioskService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<KioskViewModel> CreateNewKiosk(CreateKioskViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(model.PartyId)).FirstOrDefaultAsync();
            if(user == null)
            {
                _logger.LogInformation("Party not found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Party not found.");
            }

            var kiosk = _mapper.CreateMapper().Map<Kiosk>(model);
            kiosk.CreateDate = DateTime.Now;
            kiosk.Status = StatusConstants.DEACTIVATE;
            try
            {
                await _unitOfWork.KioskRepository.InsertAsync(kiosk);
                await _unitOfWork.SaveAsync();

                string subject = EmailUtil.getCreateKioskContent(user.Email);
                string content = EmailConstants.CREATE_KIOSK_CONTENT;
                await EmailUtil.SendEmail(user.Email, subject, content);

                var result = _mapper.CreateMapper().Map<KioskViewModel>(kiosk);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public async Task<KioskViewModel> UpdateInformation(Guid updaterId, UpdateKioskViewModel model)
        {
            var kiosk = await _unitOfWork.KioskRepository.Get(k => k.Id.Equals(model.Id)).FirstOrDefaultAsync();

            if (!kiosk.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("Your cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            kiosk.Name = model.Name;
            kiosk.Longtitude = model.Longtitude;
            kiosk.Latitude = model.Latitude;
            try
            {
                _unitOfWork.KioskRepository.Update(kiosk);
                await _unitOfWork.SaveAsync();
                var result = _mapper.CreateMapper().Map<KioskViewModel>(kiosk);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public async Task<KioskViewModel> UpdateStatus(Guid updaterId, Guid kioskId)
        {
            var kiosk = await _unitOfWork.KioskRepository.Get(k => k.Id.Equals(kioskId)).FirstOrDefaultAsync();
            if (kiosk == null)
            {
                _logger.LogInformation("Kiosk not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Kiosk not found.");
            }

            if (!kiosk.PartyId.Equals(updaterId)) // kiosk did not belong to this updater!
            {
                _logger.LogInformation("Your account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
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