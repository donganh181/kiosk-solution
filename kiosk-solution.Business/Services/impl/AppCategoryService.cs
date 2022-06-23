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
using Microsoft.Data.SqlClient;

namespace kiosk_solution.Business.Services.impl
{
    public class AppCategoryService : IAppCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAppCategoryService> _logger;
        private readonly IFileService _fileService;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;

        public AppCategoryService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IAppCategoryService> logger
            ,IFileService fileService, IPartyServiceApplicationService partyServiceApplicationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileService = fileService;
            _partyServiceApplicationService = partyServiceApplicationService;
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
            catch(SqlException e)
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

        public async Task<DynamicModelResponse<AppCategorySearchViewModel>> GetAllWithPaging(Guid? id,string role, AppCategorySearchViewModel model, int size, int pageNum)
        {
            IQueryable<AppCategorySearchViewModel> cates = null;
            List<AppCategorySearchViewModel> listCate = new List<AppCategorySearchViewModel>();           
            // if(string.IsNullOrEmpty(role) || role.Equals(RoleConstants.ADMIN) || role.Equals(RoleConstants.SERVICE_PROVIDER))
            // {
            //     cates = _unitOfWork.AppCategoryRepository
            //     .Get()
            //     .ProjectTo<AppCategorySearchViewModel>(_mapper.ConfigurationProvider);
            //     listCate = await cates.ToListAsync();
            // }
            // else if (!string.IsNullOrEmpty(role) && role.Equals(RoleConstants.LOCATION_OWNER))
            // {
            //     cates = _unitOfWork.AppCategoryRepository
            //     .Get()
            //     .ProjectTo<AppCategorySearchViewModel>(_mapper.ConfigurationProvider);
            //     var listCheck = await cates.ToListAsync();
            //
            //     foreach (var item in listCheck)
            //     {
            //         var check = await _partyServiceApplicationService.CheckAppExist(Guid.Parse(id + ""), Guid.Parse(item.Id + ""));
            //         if (check)
            //         {
            //             listCate.Add(item);
            //         }
            //     }
            //     if (listCate.Count() < 1)
            //     {
            //         _logger.LogInformation("Can not Found.");
            //         throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            //     }
            // }

            cates = listCate.AsQueryable().OrderByDescending(c => c.Name);

            var listPaging = cates.PagingIQueryable(pageNum, size,
                CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            
            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<AppCategorySearchViewModel>
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
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Invalid data.");
            }
        }
    }
}
