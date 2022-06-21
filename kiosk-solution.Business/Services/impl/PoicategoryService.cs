﻿using System;
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

        public PoicategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IPoicategoryService> logger, IFileService fileService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileService = fileService;
        }
        public async Task<PoicategoryViewModel> Create(PoiCategoryCreateViewModel model)
        {
            var cate = _mapper.Map<Poicategory>(model);
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

        public async Task<PoicategoryViewModel> UpdateName(PoiCategoryNameUpdateViewModel model)
        {
            var cate = await _unitOfWork.PoicategoryRepository.Get(c => c.Id.Equals(model.Id)).FirstOrDefaultAsync();
            if(cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
            }
            cate.Name = model.Name;
            try
            {
                _unitOfWork.PoicategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<PoicategoryViewModel>(cate);
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

        public async Task<PoicategoryViewModel> UpdateLogo(PoiCategoryLogoUpdateViewModel model)
        {
            var cate = await _unitOfWork.PoicategoryRepository.Get(c => c.Id.Equals(model.Id)).FirstOrDefaultAsync();
            if(cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
            }
            try
            {
                var newLogo = await _fileService.UploadImageToFirebase(model.Logo,
                    CommonConstants.POI_CATE_IMAGE, cate.Name, cate.Id, "Poi Cate");

                cate.Logo = newLogo;
                _unitOfWork.PoicategoryRepository.Update(cate);
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

        public async Task<PoicategoryViewModel> Delete(Guid poiCategoryId)
        {
            var cate = await _unitOfWork.PoicategoryRepository.Get(c => c.Id.Equals(poiCategoryId)).FirstOrDefaultAsync();
            if(cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
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

        public async Task<DynamicModelResponse<PoiCategorySearchViewModel>> GetAllWithPaging(PoiCategorySearchViewModel model, int size, int pageNum)
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
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
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