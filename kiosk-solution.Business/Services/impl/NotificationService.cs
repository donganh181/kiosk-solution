﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Data.Constants;
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
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<INotificationService> _logger;
        private readonly IPartyNotificationService _partyNotiService;
        private readonly INotiService _fcmService;
        private readonly IPartyService _partyService;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<INotificationService> logger, IPartyNotificationService partyNotiService,
            INotiService fcmService, IPartyService partyService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _partyNotiService = partyNotiService;
            _fcmService = fcmService;
            _partyService = partyService;
        }

        public async Task<NotificationViewModel> Create(NotificationCreateViewModel model)
        {
            var newNoti = _mapper.Map<Notification>(model);

            newNoti.CreateDate = DateTime.Now;
            newNoti.PartyNotifications = new List<PartyNotification>();
            try
            {
                await _unitOfWork.NotificationRepository.InsertAsync(newNoti);
                await _unitOfWork.SaveAsync();

                var partyNotiModel = new PartyNotificationCreateViewModel();
                partyNotiModel.PartyId = model.PartyId;
                partyNotiModel.NotificationId = newNoti.Id;

                var partyNoti = await _partyNotiService.Create(partyNotiModel);

                var party = await _partyService.GetPartyById(Guid.Parse(model.PartyId + ""));

                var deviceId = party.DeviceId;
                if (!string.IsNullOrEmpty(deviceId))
                {
                    var checkSend = await _fcmService.SendNotification(model, deviceId);
                    if (!checkSend)
                    {
                        _logger.LogInformation("Firebase Error.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase Error.");
                    }
                }

                var result = await _unitOfWork.NotificationRepository
                    .Get(n => n.Id.Equals(newNoti.Id))
                    .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<NotificationViewModel> Get(Guid id)
        {
            var noti = await _unitOfWork.NotificationRepository
                .Get(n => n.Id.Equals(id))
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return noti;
        }
    }
}
