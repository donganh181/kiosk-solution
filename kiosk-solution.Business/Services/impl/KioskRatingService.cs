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
    public class KioskRatingService : IKioskRatingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IKioskRatingService> _logger;

        public KioskRatingService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IKioskRatingService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<KioskRatingViewModel> Create(KioskRatingCreateViewModel model)
        {
            var rating = _mapper.Map<KioskRating>(model);

            rating.CreateDate = DateTime.Now;
            try
            {
                await _unitOfWork.KioskRatingRepository.InsertAsync(rating);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.KioskRatingRepository
                    .Get(r => r.Id.Equals(rating.Id))
                    .ProjectTo<KioskRatingViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<KioskRatingViewModel>> GetAllWithPagingByKioskId(Guid kioskId, int size, int pageNum)
        {
            var listPaging = _unitOfWork.KioskRatingRepository
                .Get(r => r.KioskId.Equals(kioskId))
                .ProjectTo< KioskRatingViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(r => r.CreateDate)
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging,
                CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }
            var result = new DynamicModelResponse<KioskRatingViewModel>
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

        public async Task<Dictionary<int, double?>> GetAverageRatingOfKiosk(Guid kioskId)
        {
            Dictionary<int, double?> result = new Dictionary<int, double?>();
            result.Add(0, 0);

            var listFeedback = await _unitOfWork.KioskRatingRepository
                .Get(r => r.KioskId.Equals(kioskId))
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

        public async Task<KioskRatingViewModel> GetById(Guid id)
        {
            var rating = await _unitOfWork.KioskRatingRepository
                .Get(r => r.Id.Equals(id))
                .ProjectTo<KioskRatingViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if(rating == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return rating;
        }

        public async Task<List<KioskRatingViewModel>> GetListFeedbackByKioskId(Guid kioskId)
        {
            var listFeedback = await _unitOfWork.KioskRatingRepository
                .Get(r => r.KioskId.Equals(kioskId))
                .ProjectTo<KioskRatingViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return listFeedback;
        }
    }
}
