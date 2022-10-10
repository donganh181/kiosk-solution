using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels.Map;

namespace kiosk_solution.Business.Services
{
    public interface IMapService
    {
        Task<GeocodingViewModel> GetForwardGeocode(string address);
        Task<GeocodingViewModel> GetReverseGeocode(string lat, string lng);
    }
}