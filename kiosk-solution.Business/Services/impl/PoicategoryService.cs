using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class PoicategoryService : IPoicategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPoicategoryService> _logger;
        private readonly IFileService _fileService;
        private readonly IPoiService _poiService;

        public PoicategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IPoicategoryService> logger,
            IFileService fileService, IPoiService poiService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileService = fileService;
            _poiService = poiService;
        }

        public async Task<PoicategoryViewModel> Create(PoiCategoryCreateViewModel model)
        {
            var cate = _mapper.Map<Poicategory>(model);
            var cateExist = await _unitOfWork.PoicategoryRepository.Get(c => c.Name.Equals(model.Name))
                .FirstOrDefaultAsync();
            if (cateExist != null)
            {
                _logger.LogInformation("Category name existed.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Category name existed.");
            }

            cate.Logo = "123";
            try
            {
                await _unitOfWork.PoicategoryRepository.InsertAsync(cate);
                await _unitOfWork.SaveAsync();

                var newCate = await _unitOfWork.PoicategoryRepository.Get(c => c.Id.Equals(cate.Id))
                    .FirstOrDefaultAsync();
                var logo = await _fileService.UploadImageToFirebase(model.Logo,
                    CommonConstants.POI_CATE_IMAGE, cate.Name, cate.Id, "Poi Cate");

                newCate.Logo = logo;
                _unitOfWork.PoicategoryRepository.Update(newCate);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<PoicategoryViewModel>(newCate);
                return result;
            }
            catch (SqlException e)
            {
                _logger.LogInformation(e.Message);
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, e.Message);
            }
        }

        public async Task<PoicategoryViewModel> Update(PoiCategoryUpdateViewModel model)
        {
            var cate = await _unitOfWork.PoicategoryRepository.Get(c => c.Id.Equals(model.Id)).FirstOrDefaultAsync();

            if (cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found.");
            }

            var cateExist = await _unitOfWork.PoicategoryRepository.Get(c => c.Name.Equals(model.Name) && !c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (cateExist != null)
            {
                _logger.LogInformation("Category name existed.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Category name existed.");
            }

            cate.Name = model.Name;
            try
            {
                if (!cate.Logo.Equals(model.Logo))
                {
                    var newLogo = await _fileService.UploadImageToFirebase(model.Logo,
                        CommonConstants.POI_CATE_IMAGE, cate.Name, cate.Id, "Poi Cate");
                    cate.Logo = newLogo;
                }

                _unitOfWork.PoicategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<PoicategoryViewModel>(cate);
                return result;
            }
            catch (SqlException e)
            {
                _logger.LogInformation(e.Message);
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, e.Message);
            }
        }

        public async Task<PoicategoryViewModel> Delete(Guid poiCategoryId)
        {
            var cate = await _unitOfWork.PoicategoryRepository.Get(c => c.Id.Equals(poiCategoryId))
                .FirstOrDefaultAsync();
            if (cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found.");
            }

            var existPoi = await _poiService.IsExistPoiInCategory(poiCategoryId);
            if (existPoi)
            {
                _logger.LogInformation("Already poi on this category, can not delete.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "Already poi on this category, can not delete.");
            }

            try
            {
                _unitOfWork.PoicategoryRepository.Delete(cate);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<PoicategoryViewModel>(cate);
                return result;
            }
            catch (SqlException e)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<PoiCategorySearchViewModel>> GetAllWithPaging(
            PoiCategorySearchViewModel model, int size, int pageNum)
        {
            var cates = _unitOfWork.PoicategoryRepository
                .Get()
                .ProjectTo<PoiCategorySearchViewModel>(_mapper.ConfigurationProvider)
                .DynamicFilter(model)
                .AsQueryable().OrderByDescending(c => c.Name);

            var listPaging = cates.PagingIQueryable(pageNum, size,
                CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<PoiCategorySearchViewModel>
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