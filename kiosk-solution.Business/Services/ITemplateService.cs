﻿using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface ITemplateService
    {
        Task<bool> IsOwner(Guid partyId, Guid templateId);
        Task<TemplateViewModel> Create(Guid id, TemplateCreateViewModel model);
        Task<DynamicModelResponse<TemplateSearchViewModel>> GetAllWithPaging(Guid id, TemplateSearchViewModel model, int size, int pageNum);
        Task<TemplateViewModel> UpdateInformation(Guid updaterId, TemplateUpdateViewModel model);
    }
}