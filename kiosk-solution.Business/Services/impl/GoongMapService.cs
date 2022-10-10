using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using kiosk_solution.Data.ViewModels.Map;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace kiosk_solution.Business.Services.impl
{
    public class GoongMapService : IMapService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IMapService> _logger;
        private readonly HttpClient client = new HttpClient();
        private string GongHost;
        private string GongAPIAccessKey;

        public GoongMapService(IConfiguration configuration, ILogger<IMapService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            GongHost = _configuration.GetSection("GONG_MAP")["HOST"];
            GongAPIAccessKey = _configuration.GetSection("GONG_MAP")["API_ACCESS_KEY"];
        }

        public async Task<GeocodingViewModel> GetForwardGeocode(string address)
        {
            var url = GongHost + "/geocode?address=" + address + "&api_key=" + GongAPIAccessKey;
            var res = await client.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK) return null;
            var geoMetries = new List<GeoMetryViewModel>();
            var jsonContent = res.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            var results = json["results"][0];
            var geoMetry = new GeoMetryViewModel
            {
                Address = results["formatted_address"],
                Lat = results["geometry"]["location"]["lat"],
                Lng = results["geometry"]["location"]["lng"],
                PlaceId = results["place_id"],
            };
            geoMetries.Add(geoMetry);
            return new GeocodingViewModel
            {
                GeoMetries = geoMetries
            };
        }

        public async Task<GeocodingViewModel> GetReverseGeocode(string lat, string lng)
        {
            var url = GongHost + "/Geocode?latlng=" + lat + ", " + lng + "&api_key=" + GongAPIAccessKey;
            var res = await client.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK) return null;
            var geoMetries = new List<GeoMetryViewModel>();
            var jsonContent = res.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            var results = json["results"];
            for (int i = 0; i < results.Count; i++)
            {
                var geoMetry = new GeoMetryViewModel
                {
                    Address = results[i]["formatted_address"],
                    Lat = results[i]["geometry"]["location"]["lat"],
                    Lng = results[i]["geometry"]["location"]["lng"],
                    PlaceId = results[i]["place_id"],
                };
                geoMetries.Add(geoMetry);
            }

            return new GeocodingViewModel
            {
                GeoMetries = geoMetries
            };
        }
    }
}