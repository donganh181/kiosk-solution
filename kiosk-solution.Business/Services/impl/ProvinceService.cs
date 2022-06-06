using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels.Province;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace kiosk_solution.Business.Services.impl
{
    public class ProvinceService : IProvinceService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IProvinceService> _logger;
        private readonly HttpClient client = new HttpClient();
        private string Host;
        public ProvinceService(IConfiguration configuration, ILogger<IProvinceService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            Host = _configuration.GetSection("PROVINCE")["HOST"];
        }
        public async Task<List<CityViewModel>> GetCities()
        {
            var url = Host + "/api/?depth=1";
            var res = await client.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK) return null;
            var cities = new List<CityViewModel>();
            var jsonContent = res.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            foreach (var province in json)
            {
                var city = new CityViewModel
                {
                    Code = province["code"],
                    Name = province["name"],
                    CodeName = province["codename"],
                    PhoneCode = province["phone_code"],
                    DivisionType = province["division_type"],
                };
                cities.Add(city);
            }
            return cities;
        }
        
        public async Task<List<DistrictViewModel>> GetDistrictsByCity(string cityCode)
        {
            var url = Host + "/api/p/" + cityCode + "?depth=2";
            var res = await client.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK) return null;
            var districts = new List<DistrictViewModel>();
            var jsonContent = res.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            var jsonDistricts = json["districts"];
            foreach (var x in jsonDistricts)
            {
                var district = new DistrictViewModel()
                {
                    Code = x["code"],
                    Name = x["name"],
                    CodeName = x["codename"],
                    DivisionType = x["division_type"],
                    ProvinceCode = x["province_code"]
                };
                districts.Add(district);
            }
            return districts;
        }

        public async Task<List<WardViewModel>> GetWardsByDistrict(string districtCode)
        {
            var url = Host + "/api/d/" + districtCode + "?depth=2";
            var res = await client.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK) return null;
            var wards = new List<WardViewModel>();
            var jsonContent = res.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject(jsonContent);
            var jsonWards = json["wards"];
            foreach (var x in jsonWards)
            {
                var ward = new WardViewModel()
                {
                    Code = x["code"],
                    Name = x["name"],
                    CodeName = x["codename"],
                    DivisionType = x["division_type"],
                    DistrictCode = x["district_code"]
                };
                wards.Add(ward);
            }
            return wards;
        }
    }
}