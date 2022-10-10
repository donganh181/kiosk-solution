using AutoMapper;
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
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<bool> Delete(Guid imageId)
        {
            var img = await _unitOfWork.ImageRepository.Get(i => i.Id.Equals(imageId)).FirstOrDefaultAsync();

            try
            {
                _unitOfWork.ImageRepository.Delete(img);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch(Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<ImageViewModel> GetById(Guid id)
        {
            var result = await _unitOfWork.ImageRepository
                .Get(i => i.Id.Equals(id))
                .ProjectTo<ImageViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if(result == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return result;
        }

        public async Task<List<ImageViewModel>> GetByKeyIdAndKeyType(Guid keyId, string keyType)
        {
            var result = await _unitOfWork.ImageRepository
                .Get(i => i.KeyType.Equals(keyType) && i.KeyId.Equals(keyId))
                .ProjectTo<ImageViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if(result == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
            return result;
        }

        public async Task<string> GetThumbnailByKeyIdAndKeyType(Guid keyId, string keyType)
        {
            var image = await _unitOfWork.ImageRepository
                .Get(i => i.KeyType.Equals(keyType) && keyId.Equals(keyId) && i.Link.Contains(CommonConstants.THUMBNAIL))
                .FirstOrDefaultAsync();
            
            if (image == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }

            var result = image.Link;
            if(string.IsNullOrEmpty(result))
            {
                _logger.LogInformation("Missing Data.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Missing Data.");
            }
            return result;
        }

        public async Task<ImageViewModel> Update(ImageUpdateViewModel model)
        {
            var img = await _unitOfWork.ImageRepository.Get(i => i.Id.Equals(model.Id)).FirstOrDefaultAsync();
            if(img == null)
            {
                _logger.LogInformation("Cannot found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Cannot found.");
            }
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
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }
    }
}
