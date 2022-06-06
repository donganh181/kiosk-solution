using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels.Province;

namespace kiosk_solution.Business.Services
{
    public interface IProvinceService
    {
        public Task<List<CityViewModel>> GetCities();
        public Task<List<DistrictViewModel>> GetDistrictsByCity(string cityCode);
        public Task<List<WardViewModel>> GetWardsByDistrict(string districtCode);
    }
}