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
    public class AppCategoryPositionService : IAppCategoryPositionService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IAppCategoryPositionService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITemplateService _templateService;
        private readonly IPartyServiceApplicationService _partyServiceApplicationService;

        public AppCategoryPositionService(IMapper mapper, ILogger<IAppCategoryPositionService> logger, 
            IUnitOfWork unitOfWork, IPartyServiceApplicationService partyServiceApplicationService,
            ITemplateService templateService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _partyServiceApplicationService = partyServiceApplicationService;
            _templateService = templateService;
        }

        public async Task<AppCategoryPositionViewModel> Create(Guid partyId, AppCategoryPositionCreateViewModel model)
        {
            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            if (!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(model.AppCategoryId + "")))
            {
                _logger.LogInformation($"{partyId} has no app in this category.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Your account has no app in this category.");
            }

            var position = _mapper.Map<AppCategoryPosition>(model);

            var check = await _unitOfWork.AppCategoryPositionRepository.Get(c => c.TemplateId.Equals(model.TemplateId)).ToListAsync();

            if (check != null)
            {
                //case this position already has cate
                foreach(var checkPosition in check)
                {
                    if(checkPosition.RowIndex == model.RowIndex && checkPosition.ColumnIndex == model.ColumnIndex)
                    {
                        _logger.LogInformation("This position has already had cate in template.");
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, "This position has already had cate in template.");
                    }
                }
            }    
            try
            {
                await _unitOfWork.AppCategoryPositionRepository.InsertAsync(position);
                await _unitOfWork.SaveAsync();
                var result = await _unitOfWork.AppCategoryPositionRepository
                    .Get(p => p.Id.Equals(position.Id))
                    .Include(p => p.Template)
                    .Include(p => p.AppCategory)
                    .ProjectTo<AppCategoryPositionViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
            }
        }

        public async Task<AppCategoryPositionViewModel> Update(Guid partyId, AppCategoryPositionUpdateViewModel model)
        {
            bool flag = false;
            var check = await _unitOfWork.AppCategoryPositionRepository.Get(p => p.TemplateId.Equals(model.TemplateId)).ToListAsync();

            if (!await _templateService.IsOwner(partyId, Guid.Parse(model.TemplateId + "")))
            {
                _logger.LogInformation($"{partyId} account cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            if (!await _partyServiceApplicationService.CheckAppExist(partyId, Guid.Parse(model.AppCategoryId + "")))
            {
                _logger.LogInformation($"{partyId} has no app in this category.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Your account has no app in this category.");
            }

            //case get a new cate and set it into another cate index
            if(model.Id == null) //new cate
            {
                foreach (var checkPosition in check)
                {
                    if (checkPosition.RowIndex == model.RowIndex && checkPosition.ColumnIndex == model.ColumnIndex)
                    {
                        var newPosition = _mapper.Map<AppCategoryPosition>(model);
                        newPosition.Id.Equals(checkPosition.Id);
                        try
                        {
                            _unitOfWork.AppCategoryPositionRepository.Update(newPosition); //the new cate will replace the old cate
                            await _unitOfWork.SaveAsync();
                            var result = await _unitOfWork.AppCategoryPositionRepository
                                    .Get(p => p.Id.Equals(newPosition.Id))
                                    .Include(p => p.Template)
                                    .Include(p => p.AppCategory)
                                    .ProjectTo<AppCategoryPositionViewModel>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync();
                            return result;
                        }
                        catch (Exception)
                        {
                            _logger.LogInformation("Invalid data.");
                            throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    _logger.LogInformation("You cannot update new category into an empty position. (Please use Create function).");
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You cannot update new category into an empty position. (Please use Create function).");
                }
                else
                {
                    _logger.LogInformation("Server Error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
                }
            }//case cate has already in template
            else if(model.Id != null)
            {
                var pos1 = await _unitOfWork.AppCategoryPositionRepository.Get(p => p.Id.Equals(model.Id)).FirstOrDefaultAsync();

                foreach (var pos2 in check)
                {
                    //case pos 1 and pos 2 change position
                    if (pos2.RowIndex == pos1.RowIndex && pos2.ColumnIndex == model.ColumnIndex)
                    {
                        pos2.RowIndex = pos1.RowIndex;
                        pos2.ColumnIndex = pos1.ColumnIndex;
                        pos1.RowIndex = model.RowIndex;
                        pos1.ColumnIndex = model.ColumnIndex;
                        try
                        {
                            _unitOfWork.AppCategoryPositionRepository.Update(pos1); //new cate will replace the old cate
                            _unitOfWork.AppCategoryPositionRepository.Update(pos2);
                            await _unitOfWork.SaveAsync();
                            var result = await _unitOfWork.AppCategoryPositionRepository
                                    .Get(p => p.Id.Equals(pos1.Id))
                                    .Include(p => p.Template)
                                    .Include(p => p.AppCategory)
                                    .ProjectTo<AppCategoryPositionViewModel>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync();
                            return result;
                        }
                        catch (Exception)
                        {
                            _logger.LogInformation("Invalid data.");
                            throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                //case replace cate into an empty position
                if (flag)
                {
                    try
                    {
                        _unitOfWork.AppCategoryPositionRepository.Update(pos1);
                        await _unitOfWork.SaveAsync();
                        var result = await _unitOfWork.AppCategoryPositionRepository
                                    .Get(p => p.Id.Equals(pos1.Id))
                                    .Include(p => p.Template)
                                    .Include(p => p.AppCategory)
                                    .ProjectTo<AppCategoryPositionViewModel>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync();
                        return result;
                    }
                    catch (Exception)
                    {
                        _logger.LogInformation("Invalid data.");
                        throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid data.");
                    }
                }
                else
                {
                    _logger.LogInformation("Server Error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
                }
            }
            else
            {
                _logger.LogInformation("Server Error.");
                throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Server Error.");
            }      
        }
    }
}
