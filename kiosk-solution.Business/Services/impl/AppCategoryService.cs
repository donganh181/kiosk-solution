using AutoMapper;
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
    public class AppCategoryService : IAppCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAppCategoryService> _logger;
        private readonly IFileService _fileService;

        public AppCategoryService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IAppCategoryService> logger
            ,IFileService fileService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<AppCategoryViewModel> Create(AppCategoryCreateViewModel model)
        {
            var cate = _mapper.Map<AppCategory>(model);

            try
            {
                await _unitOfWork.AppCategoryRepository.InsertAsync(cate);
                await _unitOfWork.SaveAsync();

                var newCate = await _unitOfWork.AppCategoryRepository
                    .Get(c => c.Id.Equals(cate.Id))
                    .FirstOrDefaultAsync();
                
                var logo = await _fileService.UploadImageToFirebase(model.Logo,
                    CommonConstants.CATE_IMAGE, cate.Name, cate.Id, "Cate");
               
                newCate.Logo = logo;

                _unitOfWork.AppCategoryRepository.Update(newCate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<AppCategoryViewModel>(newCate);
                return result;
            }
            catch(Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<AppCategoryViewModel> Update(AppCategoryUpdateViewModel model)
        {
            var cate = await _unitOfWork.AppCategoryRepository
                .Get(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if(cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found.");
            }

            cate.Name = model.Name;
            try
            {
                _unitOfWork.AppCategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();

                var newLogo = await _fileService.UploadImageToFirebase(model.Logo,
                    CommonConstants.CATE_IMAGE, cate.Name, cate.Id, "Cate");

                cate.Logo = newLogo;
                _unitOfWork.AppCategoryRepository.Update(cate);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<AppCategoryViewModel>(cate);
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }
    }
}
