using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
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
    public class ServiceApplicationService : IServiceApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceApplicationService> _logger;
        private readonly IFirebaseUtil _firebaseUtil;

        public ServiceApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceApplicationService> logger, IFirebaseUtil firebaseUtil)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _firebaseUtil = firebaseUtil;
        }
        public async Task<ServiceApplicationViewModel> UpdateInformation(Guid updaterId, UpdateServiceApplicationViewModel model)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(model.Id)).Include(a => a.AppCategory).FirstOrDefaultAsync();
            if (!app.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("User not match.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "You cannot use this feature.");
            }
            if(app == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
            }

            app.Name = model.Name;
            app.Description = model.Description;
            app.Link = model.Link;
            app.AppCategoryId = model.AppCategoryId;

            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();

                var newLogo = await _firebaseUtil.UploadImageToFirebase(model.Logo, app.AppCategory.Name, model.Id, "Logo");
                app.Logo = newLogo;
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceApplicationViewModel>(app);
                return result;
            }
            catch (Exception e)
            {
          
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }
    }
}
