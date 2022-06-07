﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ImageService : IImageService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IImageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public ImageService(IMapper mapper, ILogger<IImageService> logger, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ImageViewModel> Create(ImageCreateViewModel model)
        {
            var img = _mapper.Map<Image>(model);
            try
            {
                await _unitOfWork.ImageRepository.InsertAsync(img);
                await _unitOfWork.SaveAsync();

                var link = await _fileService.UploadImageToFirebase(model.Image, img.KeyType, model.KeySubType, img.Id, model.Name);
                img.Link = link;

                _unitOfWork.ImageRepository.Update(img);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<ImageViewModel>(img);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public Task<DynamicModelResponse<ImageSearchViewModel>> GetAllWithPaging(ImageSearchViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ImageViewModel> GetByLink(string link)
        {
            var result = await _unitOfWork.ImageRepository
                .Get(i => i.Link.Equals(link))
                .ProjectTo<ImageViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ImageViewModel> Update(ImageUpdateViewModel model)
        {
            var img = await _unitOfWork.ImageRepository.Get(i => i.Link.Equals(model.Link)).FirstOrDefaultAsync();

            try
            {
                var link = await _fileService.UploadImageToFirebase(model.Image, img.KeyType, model.KeySubType, img.Id, model.Name);

                img.Link = link;
                _unitOfWork.ImageRepository.Update(img);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<ImageViewModel>(img);
                return result;

            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }
    }
}