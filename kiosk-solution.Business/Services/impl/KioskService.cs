using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Hubs;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace kiosk_solution.Business.Services.impl
{
    public class KioskService : IKioskService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IKioskService> _logger;
        private readonly INotiService _fcmService;
        private readonly IEventService _eventService;
        private readonly IKioskRatingService _kioskRatingService;
        private readonly IHubContext<SystemEventHub> _eventHub;
        private readonly IPartyService _partyService;
        private readonly IKioskLocationService _kioskLocationService;

        public KioskService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IKioskService> logger
            , INotiService fcmService, IEventService eventService, IKioskRatingService kioskRatingService,
            IHubContext<SystemEventHub> eventHub, IPartyService partyService,
            IKioskLocationService kioskLocationService)
        {
            _mapper = mapper.ConfigurationProvider;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fcmService = fcmService;
            _eventService = eventService;
            _kioskRatingService = kioskRatingService;
            _eventHub = eventHub;
            _partyService = partyService;
            _kioskLocationService = kioskLocationService;
        }

        public async Task<KioskViewModel> GetByIdWithParyId(Guid kioskId, Guid partyId)
        {
            var kiosk = await _unitOfWork.KioskRepository.Get(k => k.Id.Equals(kioskId) && k.PartyId.Equals(partyId))
                .ProjectTo<KioskViewModel>(_mapper)
                .FirstOrDefaultAsync();
            return kiosk;
        }

        public async Task<KioskViewModel> AddDeviceId(KioskAddDeviceIdViewModel model)
        {
            var kiosk = await _unitOfWork.KioskRepository
                .Get(k => k.Id.Equals(model.KioskId))
                .FirstOrDefaultAsync();
            if (kiosk == null)
            {
                _logger.LogInformation("Kiosk not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Kiosk not found.");
            }

            kiosk.DeviceId = model.DeviceId;
            try
            {
                _unitOfWork.KioskRepository.Update(kiosk);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskRepository
                    .Get(k => k.Id.Equals(model.KioskId))
                    .ProjectTo<KioskViewModel>(_mapper)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<KioskViewModel> CreateNewKiosk(CreateKioskViewModel model)
        {
            var user = await _partyService.GetPartyById(Guid.Parse(model.PartyId + ""), RoleConstants.SYSTEM, null);
            if (user == null)
            {
                _logger.LogInformation("Party not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Party not found.");
            }

            if (user.RoleName != RoleConstants.LOCATION_OWNER)
            {
                _logger.LogInformation("Can not add kiosk to other role.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not add kiosk to other role.");
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
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<KioskSearchViewModel>> GetAllWithPaging(string role, Guid id,
            KioskSearchViewModel model, int size, int pageNum)
        {
            IQueryable<KioskSearchViewModel> kiosks = null;
            if (role.Equals(RoleConstants.ADMIN))
            {
                kiosks = _unitOfWork.KioskRepository.Get().ProjectTo<KioskSearchViewModel>(_mapper)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(k => k.Name);
            }


            if (role.Equals(RoleConstants.LOCATION_OWNER))
            {
                kiosks = _unitOfWork.KioskRepository.Get(k => k.PartyId.Equals(id))
                    .ProjectTo<KioskSearchViewModel>(_mapper)
                    .DynamicFilter(model)
                    .AsQueryable().OrderByDescending(k => k.Name);
            }

            if (kiosks == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var listKiosk = kiosks.ToList();

            foreach (var kiosk in listKiosk)
            {
                var rating = await _kioskRatingService.GetAverageRatingOfKiosk(Guid.Parse(kiosk.Id + ""));
                if (rating.FirstOrDefault().Value != 0)
                {
                    kiosk.NumberOfRating = rating.FirstOrDefault().Key;
                    kiosk.AverageRating = rating.FirstOrDefault().Value;
                }
                else
                {
                    kiosk.NumberOfRating = 0;
                    kiosk.AverageRating = 0;
                }

                kiosk.ListFeedback = await _kioskRatingService.GetListFeedbackByKioskId(Guid.Parse(kiosk.Id + ""));
            }

            var listPaging = listKiosk.AsQueryable()
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<KioskSearchViewModel>
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

        public async Task<KioskViewModel> GetById(Guid kioskId)
        {
            var result = await _unitOfWork.KioskRepository
                .Get(k => k.Id.Equals(kioskId))
                .ProjectTo<KioskViewModel>(_mapper)
                .FirstOrDefaultAsync();
            if (result != null)
            {
                var rating = await _kioskRatingService.GetAverageRatingOfKiosk(Guid.Parse(result.Id + ""));
                if (rating.FirstOrDefault().Value != 0)
                {
                    result.NumberOfRating = rating.FirstOrDefault().Key;
                    result.AverageRating = rating.FirstOrDefault().Value;
                }
                else
                {
                    result.NumberOfRating = 0;
                    result.AverageRating = 0;
                }

                result.ListFeedback = await _kioskRatingService.GetListFeedbackByKioskId(Guid.Parse(result.Id + ""));
            }

            return result;
        }

        public async Task<DynamicModelResponse<KioskNearbyViewModel>> GetKioskNearby(KioskNearbyViewModel model,
            int size, int pageNum)
        {
            var kiosks = _unitOfWork.KioskRepository
                .GetKioskNearBy(model.Longtitude, model.Latitude)
                .ProjectTo<KioskNearbyViewModel>(_mapper)
                .DynamicFilter(model);

            var listPaging =
                kiosks.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<KioskNearbyViewModel>
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

        public async Task<List<KioskDetailViewModel>> GetListSpecificKiosk()
        {
            var now = DateTime.Now;
            var timeNow = now.TimeOfDay;

            var thisDay = now.ToString("dddd");

            var listKiosk = _unitOfWork.KioskRepository
                .Get(k => k.Status.Equals(StatusConstants.ACTIVATE))
                .Include(a => a.KioskScheduleTemplates.Where(d => d.Status.Equals(StatusConstants.ACTIVATE)
                                                                  && d.Template.Status.Equals(StatusConstants.COMPLETE)
                                                                  && d.Schedule.DayOfWeek.Contains(thisDay)
                                                                  && d.Schedule.Status.Equals(StatusConstants
                                                                      .ON) //bỏ status
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeStart) >= 0
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeEnd) < 0
                ))
                .ThenInclude(b => b.Schedule)
                .Include(a => a.KioskScheduleTemplates.Where(d => d.Status.Equals(StatusConstants.ACTIVATE)
                                                                  && d.Template.Status.Equals(StatusConstants.COMPLETE)
                                                                  && d.Schedule.DayOfWeek.Contains(thisDay)
                                                                  && d.Schedule.Status.Equals(StatusConstants.ON)
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeStart) >= 0
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeEnd) < 0
                ))
                .ThenInclude(b => b.Template)
                .ThenInclude(c => c.AppCategoryPositions.Where(x => x != null))
                .ThenInclude(d => d.AppCategory)
                .Include(a => a.KioskScheduleTemplates.Where(d => d.Status.Equals(StatusConstants.ACTIVATE)
                                                                  && d.Template.Status.Equals(StatusConstants.COMPLETE)
                                                                  && d.Schedule.DayOfWeek.Contains(thisDay)
                                                                  && d.Schedule.Status.Equals(StatusConstants.ON)
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeStart) >= 0
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeEnd) < 0
                ))
                .ThenInclude(b => b.Template)
                .ThenInclude(c => c.EventPositions.Where(y => y != null))
                .ThenInclude(d => d.Event)
                .ToList()
                .AsQueryable()
                .ProjectTo<KioskDetailViewModel>(_mapper)
                .ToList();

            foreach (var item in listKiosk)
            {
                if (item.KioskScheduleTemplate != null)
                {
                    foreach (var eventPos in item.KioskScheduleTemplate.Template.ListEventPosition)
                    {
                        var myEvent = await _eventService.GetById(eventPos.EventId);
                        eventPos.EventThumbnail = myEvent.Thumbnail;
                    }
                }
            }

            return listKiosk;
        }

        public async Task<dynamic> GetSpecificKiosk(Guid id)
        {
            var now = DateTime.Now;
            var timeNow = now.TimeOfDay;
            var daynow = now.ToString("dddd");

            var kiosk = _unitOfWork.KioskRepository
                .Get(k => k.Status.Equals(StatusConstants.ACTIVATE) && k.Id.Equals(id))
                .Include(a => a.KioskScheduleTemplates.Where(d => d.Status.Equals(StatusConstants.ACTIVATE)
                                                                  && d.Template.Status.Equals(StatusConstants.COMPLETE)
                                                                  && d.Schedule.DayOfWeek.Contains(daynow)
                                                                  && d.Schedule.Status.Equals(StatusConstants.ON)
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeStart) >= 0
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeEnd) < 0
                ))
                .ThenInclude(b => b.Schedule)
                .Include(a => a.KioskScheduleTemplates.Where(d => d.Status.Equals(StatusConstants.ACTIVATE)
                                                                  && d.Template.Status.Equals(StatusConstants.COMPLETE)
                                                                  && d.Schedule.DayOfWeek.Contains(daynow)
                                                                  && d.Schedule.Status.Equals(StatusConstants.ON)
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeStart) >= 0
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeEnd) < 0
                ))
                .ThenInclude(b => b.Template)
                .ThenInclude(c => c.AppCategoryPositions.Where(x => x != null))
                .ThenInclude(d => d.AppCategory)
                .Include(a => a.KioskScheduleTemplates.Where(d => d.Status.Equals(StatusConstants.ACTIVATE)
                                                                  && d.Template.Status.Equals(StatusConstants.COMPLETE)
                                                                  && d.Schedule.DayOfWeek.Contains(daynow)
                                                                  && d.Schedule.Status.Equals(StatusConstants.ON)
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeStart) >= 0
                                                                  && TimeSpan.Compare(timeNow,
                                                                      (TimeSpan) d.Schedule.TimeEnd) < 0
                ))
                .ThenInclude(b => b.Template)
                .ThenInclude(c => c.EventPositions.Where(y => y != null))
                .ThenInclude(d => d.Event)
                .ToList()
                .AsQueryable()
                .ProjectTo<KioskDetailViewModel>(_mapper)
                .FirstOrDefault();

            if (kiosk == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            if (kiosk.KioskScheduleTemplate == null)
            {
                return null;
            }
            else
            {
                foreach (var eventPos in kiosk.KioskScheduleTemplate.Template.ListEventPosition)
                {
                    var myEvent = await _eventService.GetByIdIncludeDeletedStatus(eventPos.EventId);
                    eventPos.EventThumbnail = myEvent.Thumbnail;
                }

                var eventRows = new SortedDictionary<int, List<EventPositionSpecificViewModel>>();
                foreach (var eventPos in kiosk.KioskScheduleTemplate.Template.ListEventPosition)
                {
                    if (eventRows.ContainsKey((eventPos.RowIndex)))
                    {
                        eventRows[eventPos.RowIndex].Add(eventPos);
                        eventRows[eventPos.RowIndex] =
                            eventRows[eventPos.RowIndex].OrderBy(e => e.ColumnIndex).ToList();
                    }
                    else
                    {
                        var eventPositionSpecificViewModels =
                            new List<EventPositionSpecificViewModel> {eventPos};
                        eventRows.Add(eventPos.RowIndex, eventPositionSpecificViewModels);
                    }
                }

                var appRows = new SortedDictionary<int, List<AppCategoryPositionSpecificViewModel>>();
                foreach (var appPost in kiosk.KioskScheduleTemplate.Template.ListAppCatePosition)
                {
                    if (appRows.ContainsKey((appPost.RowIndex)))
                    {
                        appRows[appPost.RowIndex].Add(appPost);
                        appRows[appPost.RowIndex] = appRows[appPost.RowIndex].OrderBy(e => e.ColumnIndex).ToList();
                    }
                    else
                    {
                        var eventPositionSpecificViewModels =
                            new List<AppCategoryPositionSpecificViewModel> {appPost};
                        appRows.Add(appPost.RowIndex, eventPositionSpecificViewModels);
                    }
                }

                var kioskResult = new
                {
                    templateId = kiosk.KioskScheduleTemplate.Template.Id,
                    events = eventRows.Values,
                    appCategories = appRows.Values
                };
                var jsonConvert = JsonConvert.SerializeObject(kioskResult);
                _logger.LogInformation("<-------- start messaging to kiosk --------->");
                await _eventHub.Clients.Group(kiosk.Id.ToString())
                    .SendAsync(SystemEventHub.KIOSK_CONNECTION_CHANNEL,
                        SystemEventHub.SYSTEM_BOT, jsonConvert);
                _logger.LogInformation("<-------- end messaging to kiosk --------->");
                return kioskResult;
            }

            return kiosk;
        }

        public async Task<KioskViewModel> UpdateInformation(Guid updaterId, UpdateKioskViewModel model)
        {
            var kiosk = await _unitOfWork.KioskRepository
                .Get(k => k.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (!kiosk.PartyId.Equals(updaterId))
            {
                _logger.LogInformation("You cannot update kiosk which is not your.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot update kiosk which is not your.");
            }

            var kioskLocation = await _kioskLocationService.GetById(model.KioskLocationId, true);

            if (!kioskLocation.OwnerId.Equals(updaterId))
            {
                _logger.LogInformation("You cannot use location of other user.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "You cannot use location of other user.");
            }

            kiosk.Name = model.Name;
            kiosk.Longtitude = model.Longtitude;
            kiosk.Latitude = model.Latitude;
            kiosk.KioskLocationId = model.KioskLocationId;
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
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<KioskViewModel> UpdateStatus(Guid updaterId, Guid kioskId, bool isKioskSetup)
        {
            var kiosk = await _unitOfWork.KioskRepository
                .Get(k => k.Id.Equals(kioskId))
                .FirstOrDefaultAsync();
            if (kiosk == null)
            {
                _logger.LogInformation("Kiosk not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Kiosk not found.");
            }

            if (!kiosk.PartyId.Equals(updaterId)) // kiosk did not belong to this updater!
            {
                _logger.LogInformation("Your account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            if (!isKioskSetup)
            {
                if (kiosk.Status.Equals(StatusConstants.ACTIVATE))
                {
                    kiosk.Status = StatusConstants.DEACTIVATE;
                }

                else
                {
                    _logger.LogInformation("You cannot change to status activate.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You cannot change to status activate.");
                }
            }
            else
            {
                if (kiosk.Status.Equals(StatusConstants.ACTIVATE))
                {
                    kiosk.Status = StatusConstants.DEACTIVATE;
                }
                else
                {
                    kiosk.Status = StatusConstants.ACTIVATE;
                }
            }

            try
            {
                _unitOfWork.KioskRepository.Update(kiosk);
                await _unitOfWork.SaveAsync();
                if (!isKioskSetup)
                {
                    await _eventHub.Clients.Group(kiosk.Id.ToString())
                        .SendAsync(SystemEventHub.KIOSK_STATUS_CHANNEL,
                            SystemEventHub.SYSTEM_BOT, "CHANGE_STATUS_TO_DEACTIVATE");
                }

                var result = _mapper.CreateMapper().Map<KioskViewModel>(kiosk);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<KioskViewModel> UpdateKioskName(Guid updaterId, KioskNameUpdateViewModel model)
        {
            var kiosk = await _unitOfWork.KioskRepository
                .Get(k => k.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (kiosk == null)
            {
                _logger.LogInformation("Kiosk not found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Kiosk not found.");
            }

            if (!kiosk.PartyId.Equals(updaterId)) // kiosk did not belong to this updater!
            {
                _logger.LogInformation("Your account cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            kiosk.Name = model.KioskName;
            try
            {
                _unitOfWork.KioskRepository.Update(kiosk);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskRepository
                    .Get(k => k.Id.Equals(model.Id))
                    .ProjectTo<KioskViewModel>(_mapper)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<List<Kiosk>> GetListKioskByKioskLocationId(Guid id)
        {
            var listKiosk = await _unitOfWork.KioskRepository
                .Get(k => k.KioskLocationId.Equals(id))
                .ToListAsync();

            return listKiosk;
        }

        public async Task<CountViewModel> CountKiosks(Guid partyId, string role)
        {
            if (role.Equals(RoleConstants.ADMIN))
            {
                return new CountViewModel()
                {
                    total = await _unitOfWork.KioskRepository.Get().CountAsync(),
                    active = await _unitOfWork.KioskRepository.Get(e => e.Status.Equals(StatusConstants.ACTIVATE))
                        .CountAsync(),
                    deactive = await _unitOfWork.KioskRepository.Get(e => e.Status.Equals(StatusConstants.DEACTIVATE))
                        .CountAsync()
                };
            }
            else
            {
                return new CountViewModel()
                {
                    total = await _unitOfWork.KioskRepository.Get(e => e.PartyId.Equals(partyId)).CountAsync(),
                    active = await _unitOfWork.KioskRepository.Get(e =>
                            e.PartyId.Equals(partyId) && e.Status.Equals(StatusConstants.ACTIVATE))
                        .CountAsync(),
                    deactive = await _unitOfWork.KioskRepository.Get(e =>
                            e.PartyId.Equals(partyId) && e.Status.Equals(StatusConstants.DEACTIVATE))
                        .CountAsync()
                };
            }
        }

        public async Task<string> GetNameById(Guid kioskId)
        {
            var kiosk = await _unitOfWork.KioskRepository.Get(k => k.Id.Equals(kioskId)).FirstOrDefaultAsync();
            return kiosk.Name;
        }
    }
}