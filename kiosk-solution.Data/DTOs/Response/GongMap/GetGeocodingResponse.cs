using System.Collections.Generic;

namespace kiosk_solution.Data.DTOs.Response.GongMap
{
    public class GeoMetry
    {
        public string PlaceId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
    }

    public class GetGeocodingResponse
    {
        public List<GeoMetry> GeoMetries { get; set; }
    }
}