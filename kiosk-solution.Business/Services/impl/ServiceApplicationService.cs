using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
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
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using Microsoft.Data.SqlClient;

namespace kiosk_solution.Business.Services.impl
{
    public class ServiceApplicationService : IServiceApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IServiceApplicationService> _logger;
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;

        public ServiceApplicationService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<ServiceApplicationService> logger, IFileService fileService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
            _notificationService = notificationService;
        }

        public async Task<DynamicModelResponse<ServiceApplicationSearchViewModel>> GetAllWithPaging(string role,
            Guid? partyId, ServiceApplicationSearchViewModel model, int size, int pageNum)
        {
            object apps = null;
            if (string.IsNullOrEmpty(role) || role.Equals(RoleConstants.ADMIN))
            {
                apps = _unitOfWork.ServiceApplicationRepository
                    .Get()
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .ProjectTo<ServiceApplicationSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(a => a.Name);
            }

            if (!string.IsNullOrEmpty(role) && role.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                apps = _unitOfWork.ServiceApplicationRepository
                    .Get(a => a.PartyId.Equals(partyId))
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .ProjectTo<ServiceApplicationSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(a => a.CreateDate);
            }

            if (!string.IsNullOrEmpty(role) && role.Equals(RoleConstants.LOCATION_OWNER))
            {
                apps = _unitOfWork.ServiceApplicationRepository
                    .Get()
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .Include(a => a.PartyServiceApplications.Where(x => x.PartyId.Value == partyId))
                    .ThenInclude(b => b.Party)
                    .Include(a => a.PartyServiceApplications.Where(x => x.PartyId.Value == partyId))
                    .ThenInclude(b => b.ServiceApplication)
                    .ThenInclude(c => c.AppCategory)
                    .ToList()
                    .AsQueryable()
                    .ProjectTo<ServiceApplicationSearchViewModel>(_mapper.ConfigurationProvider)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(a => a.CreateDate);
            }

            if (apps == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var listApp = (IQueryable<ServiceApplicationSearchViewModel>) apps;

            var listPaging =
                listApp.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }


            var result = new DynamicModelResponse<ServiceApplicationSearchViewModel>
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

        public async Task<ServiceApplicationViewModel> UpdateInformation(Guid updaterId,
            UpdateServiceApplicationViewModel model)
        {
            var app = await _unitOfWork.ServiceApplicationRepository
                .Get(a => a.Id.Equals(model.Id))
                .Include(a => a.Party)
                .Include(a => a.AppCategory)
                .FirstOrDefaultAsync();
            if (!app.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("User not match.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot use this feature.");
            }

            if (app == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found.");
            }

            app.Name = model.Name;
            app.Description = model.Description;
            app.Link = model.Link;
            app.AppCategoryId = model.AppCategoryId;

            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();
                if (model.Logo != null)
                {
                    var newLogo =
                        await _fileService.UploadImageToFirebase(model.Logo, CommonConstants.APP_IMAGE,
                            app.AppCategory.Name, model.Id, "Logo");
                    app.Logo = newLogo;
                    _unitOfWork.ServiceApplicationRepository.Update(app);
                    await _unitOfWork.SaveAsync();
                }

                var result = _mapper.Map<ServiceApplicationViewModel>(app);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<ServiceApplicationViewModel> Create(Guid partyId, CreateServiceApplicationViewModel model)
        {
            var serviceApplication = _mapper.Map<ServiceApplication>(model);
            serviceApplication.PartyId = partyId;
            serviceApplication.Status = StatusConstants.INCOMPLETE;
            serviceApplication.CreateDate = DateTime.Now;
            try
            {
                await _unitOfWork.ServiceApplicationRepository.InsertAsync(serviceApplication);
                await _unitOfWork.SaveAsync();

                var serviceApplicationNew = await _unitOfWork.ServiceApplicationRepository
                    .Get(a => a.Id.Equals(serviceApplication.Id))
                    .Include(a => a.AppCategory)
                    .Include(a => a.Party)
                    .FirstOrDefaultAsync();
                var logo = await _fileService.UploadImageToFirebase(model.Logo, CommonConstants.APP_IMAGE,
                    serviceApplicationNew.AppCategory.Name,
                    serviceApplication.Id, "Logo");
                serviceApplication.Logo = logo;
                serviceApplication.Status = StatusConstants.UNAVAILABLE;
                _unitOfWork.ServiceApplicationRepository.Update(serviceApplicationNew);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<ServiceApplicationViewModel>(serviceApplicationNew);
                return result;
            }
            catch (SqlException e)
            {
                if (e.Number == 2601)
                {
                    _logger.LogInformation("Name is duplicated.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Name is duplicated.");
                }
                else
                {
                    _logger.LogInformation("Invalid Data.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
                }
            }
        }

        public async Task<ServiceApplicationViewModel> GetById(Guid id)
        {
            return await _unitOfWork.ServiceApplicationRepository
                .Get(a => a.Id.Equals(id))
                .Include(a => a.Party)
                .Include(a => a.AppCategory)
                .ProjectTo<ServiceApplicationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SetStatus(Guid id, string status)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            if (app == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            app.Status = status;

            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<bool> HasApplicationOnCategory(Guid appCategoryId)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(s => s.AppCategoryId.Equals(appCategoryId))
                .FirstOrDefaultAsync();
            if (app == null)
                return false;
            else
                return true;
        }

        public async Task<ServiceApplicationViewModel> UpdateStatus(ServiceApplicationUpdateStatusViewModel model)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(model.serviceApplicationId))
                .FirstOrDefaultAsync();
            if (app == null)
            {
                _logger.LogInformation("App not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "App not found.");
            }

            if (!app.Status.Equals(StatusConstants.AVAILABLE))
            {
                _logger.LogInformation("This status app can not update.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "This status app can not update.");
            }

            app.Status = StatusConstants.UNAVAILABLE;
            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(app);
                await _unitOfWork.SaveAsync();
                var notiModel = new NotificationCreateViewModel()
                {
                    PartyId = (Guid) app.PartyId,
                    Title = "Your application has been stopped",
                    Content = $"Your application {app.Name} has been stopped by admin."
                };
                await _notificationService.Create(notiModel);
                var result = _mapper.Map<ServiceApplicationViewModel>(app);
                return result;

            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}