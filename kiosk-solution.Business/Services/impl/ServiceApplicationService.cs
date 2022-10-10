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
        private readonly IServiceApplicationFeedBackService _serviceApplicationFeedBackService;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;

        public ServiceApplicationService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<ServiceApplicationService> logger, IFileService fileService,
            INotificationService notificationService,
            IServiceApplicationFeedBackService serviceApplicationFeedBackService,
            IPartyServiceApplicationService partyServiceApplicationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
            _notificationService = notificationService;
            _serviceApplicationFeedBackService = serviceApplicationFeedBackService;
            _partyServiceApplicationService = partyServiceApplicationService;
        }

        public async Task<ServiceApplicationViewModel> UpdateBanner(Guid partyId, ServiceApplicationUpdateBannerViewModel model)
        {
            var appUpdate = await _unitOfWork.ServiceApplicationRepository
                .Get(e => e.Id.Equals(model.ServiceApplicationId) && e.PartyId.Equals(partyId)).FirstOrDefaultAsync();
            if (appUpdate == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }
            if (string.IsNullOrEmpty(model.Banner))
            {
                appUpdate.Banner = null;
            }else if(model.Banner.Equals(appUpdate.Banner))
            {
                _logger.LogInformation("Do not thing.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Do not thing.");
            }else
            {
                var link = await _fileService.UploadImageToFirebase(model.Banner,CommonConstants.BANNER_IMAGE, CommonConstants.BANNER_APP, appUpdate.Id, appUpdate.Name);
                appUpdate.Banner = link;
            }
            try
            {
                _unitOfWork.ServiceApplicationRepository.Update(appUpdate);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<ServiceApplicationViewModel>(appUpdate);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<DynamicModelResponse<ServiceApplicationSearchViewModel>> GetAllWithPaging(string role,
            Guid? partyId, ServiceApplicationSearchViewModel model, int size, int pageNum)
        {
            IQueryable<ServiceApplicationSearchViewModel> apps = null;
            var userCounter = 0;
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

            var listApp = apps.ToList();


            foreach (var app in listApp)
            {
                var rating = await _serviceApplicationFeedBackService.GetAverageRatingOfApp(Guid.Parse(app.Id + ""));
                if (rating.FirstOrDefault().Value != 0)
                {
                    app.NumberOfRating = rating.FirstOrDefault().Key;
                    app.AverageRating = rating.FirstOrDefault().Value;
                }
                else
                {
                    app.NumberOfRating = 0;
                    app.AverageRating = 0;
                }

                app.ListFeedback =
                    await _serviceApplicationFeedBackService.GetListFeedbackByAppId(Guid.Parse(app.Id + ""));

                userCounter = await _partyServiceApplicationService.CountUserByAppId(Guid.Parse(app.Id + ""));
                app.UserInstalled = userCounter;
            }

            var listPaging =
                listApp.AsQueryable().PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                    CommonConstants.DefaultPaging);

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
            app.IsAffiliate = model.IsAffiliate;

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
            catch (Exception)
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
            serviceApplication.Banner = null;
            try
            {
                await _unitOfWork.ServiceApplicationRepository.InsertAsync(serviceApplication);
                await _unitOfWork.SaveAsync();
                if (model.Banner != null && model.Banner.Length > 0)
                {
                    var link = await _fileService.UploadImageToFirebase(model.Banner,CommonConstants.BANNER_IMAGE, CommonConstants.BANNER_APP, serviceApplication.Id, serviceApplication.Name);
                    serviceApplication.Banner = link;
                }
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
            catch (Exception e)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<ServiceApplicationSpecificViewModel> GetById(Guid? partyId, Guid id)
        {
            var userCounter = 0;
            ServiceApplicationSpecificViewModel app = null;
            if (partyId == null)
            {
                app = _unitOfWork.ServiceApplicationRepository
                    .Get(a => a.Id.Equals(id))
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .ToList()
                    .AsQueryable()
                    .ProjectTo<ServiceApplicationSpecificViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefault();
            }
            else
            {
                app = _unitOfWork.ServiceApplicationRepository
                    .Get(a => a.Id.Equals(id))
                    .Include(a => a.Party)
                    .Include(a => a.AppCategory)
                    .Include(a =>
                        a.ServiceApplicationFeedBacks.Where(x =>
                            x.PartyId.Value == partyId && x.ServiceApplicationId.Value == id))
                    .ThenInclude(b => b.Party)
                    .Include(a =>
                        a.ServiceApplicationFeedBacks.Where(x =>
                            x.PartyId.Value == partyId && x.ServiceApplicationId.Value == id))
                    .ThenInclude(b => b.ServiceApplication)
                    .ToList()
                    .AsQueryable()
                    .ProjectTo<ServiceApplicationSpecificViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefault();
            }

            if (app != null)
            {
                var rating = await _serviceApplicationFeedBackService.GetAverageRatingOfApp(Guid.Parse(app.Id + ""));
                if (rating.FirstOrDefault().Value != 0)
                {
                    app.NumberOfRating = rating.FirstOrDefault().Key;
                    app.AverageRating = rating.FirstOrDefault().Value;
                }
                else
                {
                    app.NumberOfRating = 0;
                    app.AverageRating = 0;
                }

                app.ListFeedback =
                    await _serviceApplicationFeedBackService.GetListFeedbackByAppId(Guid.Parse(app.Id + ""));

                userCounter = await _partyServiceApplicationService.CountUserByAppId(Guid.Parse(app.Id + ""));
                app.UserInstalled = userCounter;
            }

            return app;
        }

        public async Task<string> GetNameById(Guid serviceApplicationId)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(s => s.Id.Equals(serviceApplicationId))
                .FirstOrDefaultAsync();
            return app.Name;
        }

        public async Task<ServiceApplicationCommissionViewModel> GetCommissionById(Guid serviceApplicationId)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(serviceApplicationId))
                .ProjectTo<ServiceApplicationCommissionViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return app;
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
            var app = await _unitOfWork.ServiceApplicationRepository
                .Get(a => a.Id.Equals(model.serviceApplicationId))
                .Include(x => x.Party)
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

                var subject = EmailConstants.STOP_APP_SUBJECT;
                var content = EmailUtil.GetStopAppContent(app.Name);
                await EmailUtil.SendEmail(app.Party.Email, subject, content);

                var result = _mapper.Map<ServiceApplicationViewModel>(app);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<CountViewModel> CountApps(Guid partyId, string role)
        {
            if (role.Equals(RoleConstants.ADMIN))
            {
                return new CountViewModel()
                {
                    total = await _unitOfWork.ServiceApplicationRepository.Get().CountAsync(),
                    active = await _unitOfWork.ServiceApplicationRepository
                        .Get(e => e.Status.Equals(StatusConstants.AVAILABLE))
                        .CountAsync(),
                    deactive = await _unitOfWork.ServiceApplicationRepository
                        .Get(e => e.Status.Equals(StatusConstants.UNAVAILABLE))
                        .CountAsync()
                };
            }

            if (role.Equals(RoleConstants.LOCATION_OWNER))
            {
                return new CountViewModel()
                {
                    total = await _unitOfWork.PartyServiceApplicationRepository.Get(e => e.PartyId.Equals(partyId)).CountAsync(),
                    active = await _unitOfWork.PartyServiceApplicationRepository.Get(e =>
                            e.PartyId.Equals(partyId) && e.Status.Equals(StatusConstants.INSTALLED))
                        .CountAsync(),
                    deactive = await _unitOfWork.PartyServiceApplicationRepository.Get(e =>
                            e.PartyId.Equals(partyId) && e.Status.Equals(StatusConstants.UNINSTALLED))
                        .CountAsync()
                };
            }
            return new CountViewModel()
            {
                total = await _unitOfWork.ServiceApplicationRepository.Get(e => e.PartyId.Equals(partyId)).CountAsync(),
                active = await _unitOfWork.ServiceApplicationRepository.Get(e =>
                        e.PartyId.Equals(partyId) && e.Status.Equals(StatusConstants.AVAILABLE))
                    .CountAsync(),
                deactive = await _unitOfWork.ServiceApplicationRepository.Get(e =>
                        e.PartyId.Equals(partyId) && e.Status.Equals(StatusConstants.UNAVAILABLE))
                    .CountAsync()
            };
        }

        public async Task<bool> GetAffiliateByAppId(Guid appId)
        {
            var app = await _unitOfWork.ServiceApplicationRepository.Get(a => a.Id.Equals(appId)).FirstOrDefaultAsync();

            if(app == null)
            {
                return false;
            }else
            {
                return bool.Parse(app.IsAffiliate+"");
            }
        }
    }
}