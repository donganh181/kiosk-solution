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
    public class ServiceApplicationFeedBackService : IServiceApplicationFeedBackService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IServiceApplicationFeedBackService> _logger;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;

        public ServiceApplicationFeedBackService(IMapper mapper, IUnitOfWork unitOfWork,
            ILogger<IServiceApplicationFeedBackService> logger, IPartyServiceApplicationService partyServiceApplicationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _partyServiceApplicationService = partyServiceApplicationService;
        }

        public async Task<ServiceApplicationFeedBackViewModel> Create(Guid partyId, ServiceApplicationFeedBackCreateViewModel model)
        {
            var checkAppExist = await _partyServiceApplicationService.CheckAppExistByPartyIdAndServiceApplicationId(partyId, model.ServiceApplicationId);
            if (!checkAppExist)
            {
                _logger.LogInformation("You have not installed this app before. So you cannot feedback this app.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You have not installed this app before. So you cannot feedback this app.");

            }

            var check = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.PartyId.Equals(partyId) && c.ServiceApplicationId.Equals(model.ServiceApplicationId))
                .FirstOrDefaultAsync();

            if(check != null)
            {
                _logger.LogInformation("This user has feedback for this app.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This user has feedback for this app.");
            }

            var feedback = _mapper.Map<ServiceApplicationFeedBack>(model);

            feedback.CreateDate = DateTime.Now;
            feedback.PartyId = partyId;
            try
            {
                await _unitOfWork.ServiceApplicationFeedBackRepository.InsertAsync(feedback);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.Id.Equals(feedback.Id))
                .Include(c => c.Party)
                .Include(c => c.ServiceApplication)
                .ProjectTo<ServiceApplicationFeedBackViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<Dictionary<int, double?>> GetAverageRatingOfApp(Guid appId)
        {
            Dictionary<int, double?> result = new Dictionary<int, double?>();
            result.Add(0, 0);
            
            var listFeedback = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.ServiceApplicationId.Equals(appId))
                .ToListAsync();

            if (listFeedback.Count == 0) return result;

            result.Clear();

            double rating = 0;
            int total = 0;
            foreach (var feedback in listFeedback)
            {
                if (feedback != null)
                {
                    if (feedback.Rating == null || feedback.Rating > 0)
                    {
                        total++;
                        rating += (float)feedback.Rating;
                    }
                }
            }

            result.Add(total, rating / total);
            return result;
        }

        public async Task<ServiceApplicationFeedBackViewModel> GetFeedbackById(Guid id)
        {
            var feedback = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.Id.Equals(id))
                .ProjectTo<ServiceApplicationFeedBackViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (feedback == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return feedback;
        }

        public async Task<List<ServiceApplicationFeedBackViewModel>> GetListFeedbackByAppId(Guid appId)
        {
            var listFeedback = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(f => f.ServiceApplicationId.Equals(appId))
                .Include(x => x.Party)
                .Include(x => x.ServiceApplication)
                .ProjectTo<ServiceApplicationFeedBackViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return listFeedback;
        }

        public async Task<DynamicModelResponse<ServiceApplicationFeedBackViewModel>> GetListFeedbackByAppIdWithPaging(Guid appId, int size, int pageNum)
        {
            var listPaging = _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.ServiceApplicationId.Equals(appId))
                .Include(x => x.Party)
                .Include(x => x.ServiceApplication)
                .ProjectTo<ServiceApplicationFeedBackViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(r => r.CreateDate)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<ServiceApplicationFeedBackViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total,
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<ServiceApplicationFeedBackViewModel> Update(Guid partyId, ServiceApplicationFeedBackUpdateViewModel model)
        {
            var feedback = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if(feedback == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            if (!feedback.PartyId.Equals(partyId))
            {
                _logger.LogInformation("Cannot update feedback of other user.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot update feedback of other user.");
            }

            feedback.Content = model.Content;
            feedback.Rating = model.Rating;
            try
            {
                _unitOfWork.ServiceApplicationFeedBackRepository.Update(feedback);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.ServiceApplicationFeedBackRepository
                .Get(c => c.Id.Equals(feedback.Id))
                .Include(c => c.Party)
                .Include(c => c.ServiceApplication)
                .ProjectTo<ServiceApplicationFeedBackViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

    }
}
