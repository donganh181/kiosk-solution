using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using kiosk_solution.Data.DTOs.Response.GongMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace kiosk_solution.Business.Services.impl
{
    public class GoongMapService : IMapService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IPartyService> _logger;
        private readonly HttpClient client = new HttpClient();
        private string GongHost;
        private string GongAPIAccessKey;

        public GoongMapService(IConfiguration configuration, ILogger<IPartyService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            GongHost = _configuration.GetSection("GONG_MAP")["HOST"];
            GongAPIAccessKey = _configuration.GetSection("GONG_MAP")["API_ACCESS_KEY"];
        }

        public async Task<GetGeocodingResponse> GetForwardGeocode(string address)
        {
            var url = GongHost + "/geocode?address=" + address + "&api_key=" + GongAPIAccessKey;
            var res = await client.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK) return null;
            var geoMetries = new List<GeoMetry>();
            var jsonContent = res.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            var results = json["results"][0];
            var geoMetry = new GeoMetry
            {
                Address = results["formatted_address"],
                Lat = results["geometry"]["location"]["lat"],
                Lng = results["geometry"]["location"]["lng"],
                PlaceId = results["place_id"],
            };
            geoMetries.Add(geoMetry);
            return new GetGeocodingResponse
            {
                GeoMetries = geoMetries
            };
        }
    }
}