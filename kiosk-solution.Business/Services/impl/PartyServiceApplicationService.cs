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
    public class PartyServiceApplicationService : IPartyServiceApplicationService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IPartyServiceApplicationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PartyServiceApplicationService(IMapper mapper, ILogger<IPartyServiceApplicationService> logger,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckAppExist(Guid partyId, Guid cateId)
        {
            bool check = false;

            var listApp = await _unitOfWork.PartyServiceApplicationRepository
                .Get(a => a.PartyId.Equals(partyId))
                .Include(a => a.ServiceApplication)
                .ToListAsync();

            if (listApp != null)
            {
                foreach (var app in listApp)
                {
                    if (app.ServiceApplication.AppCategoryId.Equals(cateId))
                    {
                        check = true;
                    }
                }
            }

            return check;
        }

        public async Task<PartyServiceApplicationViewModel> UpdateStatus(Guid partyId, PartyServiceApplicationUpdateViewModel model)
        {
            var partyApp = await _unitOfWork.PartyServiceApplicationRepository.Get(x =>
                    x.PartyId.Equals(partyId) && x.ServiceApplicationId.Equals(model.serviceApplication))
                .FirstOrDefaultAsync();
            if (partyApp == null)
            {
                _logger.LogInformation("Can not found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            partyApp.Status = partyApp.Status.Equals(StatusConstants.INSTALLED) ? StatusConstants.UNINSTALLED : StatusConstants.INSTALLED;
            try
            {
                _unitOfWork.PartyServiceApplicationRepository.Update(partyApp);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.PartyServiceApplicationRepository
                    .Get(a => a.Id.Equals(partyApp.Id))
                    .Include(a => a.Party)
                    .Include(a => a.ServiceApplication)
                    .ThenInclude(b => b.AppCategory)
                    .ProjectTo<PartyServiceApplicationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception )
            {
                _logger.LogInformation("Fail to update.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Fail to update.");
            }
        }

        public async Task<PartyServiceApplicationViewModel> Create(Guid id,
            PartyServiceApplicationCreateViewModel model)
        {
            var checkExist = await _unitOfWork.PartyServiceApplicationRepository
                .Get(c => c.PartyId.Equals(id) && c.ServiceApplicationId.Equals(model.ServiceApplicationId))
                .FirstOrDefaultAsync();
            if (checkExist != null)
            {
                _logger.LogInformation("You have already taken this app.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You have already taken this app.");
            }
            
            var partyService = _mapper.Map<PartyServiceApplication>(model);
            partyService.Status = StatusConstants.INSTALLED;
            partyService.PartyId = id;

            partyService.Status = ServiceApplicationConstants.INSTALLED;

            try
            {
                await _unitOfWork.PartyServiceApplicationRepository.InsertAsync(partyService);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.PartyServiceApplicationRepository
                    .Get(a => a.Id.Equals(partyService.Id))
                    .Include(a => a.Party)
                    .Include(a => a.ServiceApplication)
                    .ThenInclude(b => b.AppCategory)
                    .ProjectTo<PartyServiceApplicationViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<PartyServiceApplicationSearchViewModel>> GetAllWithPaging(Guid id,
            PartyServiceApplicationSearchViewModel model, int size, int pageNum)
        {
            var apps = _unitOfWork.PartyServiceApplicationRepository
                .Get(a => a.PartyId.Equals(id))
                .Include(a => a.Party)
                .Include(a => a.ServiceApplication)
                .ThenInclude(b => b.AppCategory)
                .ProjectTo<PartyServiceApplicationSearchViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter(model)
                .AsQueryable().OrderByDescending(a => a.ServiceApplicationName);

            var listPaging = apps.PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<PartyServiceApplicationSearchViewModel>
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
    }
}
