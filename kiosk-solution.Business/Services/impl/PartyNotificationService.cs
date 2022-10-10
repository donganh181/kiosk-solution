using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
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
    public class PartyNotificationService : IPartyNotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPartyNotificationService> _logger;

        public PartyNotificationService(IMapper mapper, IUnitOfWork unitOfWork,
            ILogger<IPartyNotificationService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PartyNotificationViewModel> Create(PartyNotificationCreateViewModel model)
        {
            var newPartyNoti = _mapper.Map<PartyNotification>(model);

            newPartyNoti.Status = NotificationConstants.UNSEEN;
            try
            {
                await _unitOfWork.PartyNotificationRepository.InsertAsync(newPartyNoti);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.PartyNotificationRepository
                    .Get(n => n.Id.Equals(newPartyNoti.Id))
                    .Include(n => n.Party)
                    .Include(n => n.Notification)
                    .ProjectTo<PartyNotificationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }

        public async Task<NotificationDynamicModelResponse<PartyNotificationViewModel>> Get(Guid partyId, int size, int pageNum)
        {
            var listPaging = _unitOfWork.PartyNotificationRepository
                .Get(n => n.PartyId.Equals(partyId))
                .Include(n => n.Party)
                .Include(n => n.Notification)
                .ProjectTo<PartyNotificationViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(n => n.NotiCreateDate)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            var listUnseen = await _unitOfWork.PartyNotificationRepository
                .Get(n => n.PartyId.Equals(partyId) && n.Status.Equals(NotificationConstants.UNSEEN))
                .ToListAsync();

            int unseenNoti = listUnseen.Count;

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new NotificationDynamicModelResponse<PartyNotificationViewModel>
            {
                Metadata = new NotificationPagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total,
                    UnseenNoti = unseenNoti
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<PartyNotificationViewModel> GetById(Guid partyid, Guid partyNotiId)
        {
            var noti = await _unitOfWork.PartyNotificationRepository
                .Get(n => n.Id.Equals(partyNotiId) && n.PartyId.Equals(partyid))
                .FirstOrDefaultAsync();

            if(noti == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            noti.Status = NotificationConstants.SEEN;

            try
            {
                _unitOfWork.PartyNotificationRepository.Update(noti);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.PartyNotificationRepository
                    .Get(n => n.Id.Equals(partyNotiId) && n.PartyId.Equals(partyid))
                    .Include(n => n.Party)
                    .Include(n => n.Notification)
                    .ProjectTo<PartyNotificationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}
