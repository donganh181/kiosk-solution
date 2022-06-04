using System.Threading.Tasks;
using kiosk_solution.Data.DTOs.Response.GongMap;

namespace kiosk_solution.Business.Services
{
    public interface IMapService
    {
        Task<GetGeocodingResponse> GetForwardGeocode(string address);
        Task<GetGeocodingResponse> GetReverseGeocode(string lat, string lng);
    }
}