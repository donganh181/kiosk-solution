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
        private readonly IServiceApplicationService _serviceApplicationService;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;

        public AppCategoryService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<IAppCategoryService> logger,
            IFileService fileService, IServiceApplicationService serviceApplicationService,
            IPartyServiceApplicationService partyServiceApplicationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileService = fileService;
            _serviceApplicationService = serviceApplicationService;
            _partyServiceApplicationService = partyServiceApplicationService;
        }

        public async Task<AppCategoryViewModel> Create(AppCategoryCreateViewModel model)
        {
            var cate = _mapper.Map<AppCategory>(model);
            var cateExist = await _unitOfWork.AppCategoryRepository.Get(c => c.Name.Equals(model.Name))
                .FirstOrDefaultAsync();
            if (cateExist != null)
            {
                _logger.LogInformation("Category name existed.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Category name existed.");
            }

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
            catch (SqlException e)
            {
                _logger.LogInformation(e.Message);
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, e.Message);
            }
        }

        public async Task<AppCategoryViewModel> Delete(AppCategoryDeleteViewModel model)
        {
            var cate = await _unitOfWork.AppCategoryRepository.Get(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found.");
            }
            var checkExist = await _serviceApplicationService.HasApplicationOnCategory(model.Id);
            if (checkExist)
            {
                _logger.LogInformation("Already app on this category, can not delete.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest,
                    "Already app on this category, can not delete.");
            }
            else
            {
                var appCate = await _unitOfWork.AppCategoryRepository.Get(a => a.Id.Equals(model.Id))
                    .FirstOrDefaultAsync();
                _unitOfWork.AppCategoryRepository.Delete(appCate);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<AppCategoryViewModel>(appCate);
                return result;
            }
        }

        public async Task<DynamicModelResponse<AppCategorySearchViewModel>> GetAllWithPaging(Guid? id, string role,
            AppCategorySearchViewModel model, int size, int pageNum)
        {
            //
            IQueryable<AppCategorySearchViewModel> cates = null;
            List<AppCategorySearchViewModel> listCate = new List<AppCategorySearchViewModel>();
            if (string.IsNullOrEmpty(role) || role.Equals(RoleConstants.ADMIN) ||
                role.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                cates = _unitOfWork.AppCategoryRepository
                    .Get()
                    .ProjectTo<AppCategorySearchViewModel>(_mapper.ConfigurationProvider);
                listCate = await cates.ToListAsync();
            }
            else if (!string.IsNullOrEmpty(role) && role.Equals(RoleConstants.LOCATION_OWNER))
            {
                cates = _unitOfWork.AppCategoryRepository
                    .Get(c=>!c.Name.Equals("Transport") && !c.Name.Equals("Food"))
                    .ProjectTo<AppCategorySearchViewModel>(_mapper.ConfigurationProvider);
                var listCheck = await cates.ToListAsync();

                foreach (var item in listCheck)
                {
                    var check = await _partyServiceApplicationService.CheckAppExist(Guid.Parse(id + ""),
                        Guid.Parse(item.Id + ""));
                    if (check)
                    {
                        listCate.Add(item);
                    }
                }

                if (listCate.Count() < 1)
                {
                    _logger.LogInformation("Can not Found.");
                    throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
                }
            }

            cates = listCate.AsQueryable().OrderByDescending(c => c.Name);

            var listPaging = cates.PagingIQueryable(pageNum, size,
                CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
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

        public async Task<AppCategoryViewModel> GetById(Guid id)
        {
            var cate = await _unitOfWork.AppCategoryRepository
                .Get(c => c.Id.Equals(id))
                .ProjectTo<AppCategoryViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            return cate;
        }

        public async Task<AppCategoryViewModel> Update(AppCategoryUpdateViewModel model)
        {
            var cate = await _unitOfWork.AppCategoryRepository
                .Get(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            if (cate == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found.");
            }

            var cateExist = await _unitOfWork.AppCategoryRepository.Get(c => c.Name.Equals(model.Name)&&!c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();
            if (cateExist != null)
            {
                _logger.LogInformation("Category name existed.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Category name existed.");
            }

            cate.Name = model.Name;
            cate.CommissionPercentage = model.CommissionPercentage;
            try
            {
                if (!cate.Logo.Equals(model.Logo))
                {
                    var newLogo = await _fileService.UploadImageToFirebase(model.Logo,
                        CommonConstants.CATE_IMAGE, cate.Name, cate.Id, "Cate");
                    cate.Logo = newLogo;
                }

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