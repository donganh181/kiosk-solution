using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IImageService
    {
        public Task<ImageViewModel> Create(ImageCreateViewModel model);
        public Task<ImageViewModel> Update(ImageUpdateViewModel model);
        public Task<ImageViewModel> GetByKeyIdAndKeyType(Guid keyId, string keyType);
        public Task<DynamicModelResponse<ImageSearchViewModel>> GetAllWithPaging(ImageSearchViewModel model);
    }
}
