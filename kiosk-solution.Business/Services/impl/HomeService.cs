using kiosk_solution.Data.Constants;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services.impl
{
    public class HomeService : IHomeService
    {
        private readonly ILogger<IHomeService> _logger;
        private readonly IPartyServiceApplicationService _appService;
        private readonly IServiceApplicationFeedBackService _feedbackService;
        private readonly IPoiService _poiService;
        private readonly IEventService _eventService;
        private readonly IKioskService _kioskService;

        public HomeService(ILogger<IHomeService> logger,
            IPartyServiceApplicationService appService,
            IPoiService poiService, IEventService eventService,
            IServiceApplicationFeedBackService feedbackService,
            IKioskService kioskService)
        {
            _logger = logger;
            _appService = appService;
            _poiService = poiService;
            _eventService = eventService;
            _feedbackService = feedbackService;
            _kioskService = kioskService;
        }

        public async Task<List<SlideViewModel>> GetListHomeImage(Guid partyId, Guid kioskId)
        {
            var listSlide = new List<SlideViewModel>();

            var kiosk = await _kioskService.GetById(kioskId);

            var listApp = await _appService.GetListAppByPartyId(partyId);

            if (listApp.Count > (int)BannerConstants.NOT_MEET)
            {
                foreach (var item in listApp)
                {
                    var rating = await _feedbackService.GetAverageRatingOfApp(Guid.Parse(item.ServiceApplicationId + ""));
                    if (rating.FirstOrDefault().Value != 0)
                    {
                        item.ServiceAppModel.NumberOfRating = rating.FirstOrDefault().Key;
                        item.ServiceAppModel.AverageRating = rating.FirstOrDefault().Value;
                    }
                    else
                    {
                        item.ServiceAppModel.NumberOfRating = 0;
                        item.ServiceAppModel.AverageRating = 0;
                    }
                }

                var listAppByRating = listApp.AsQueryable().OrderByDescending(a => a.ServiceAppModel.AverageRating).ToList();

                for(int i = 0; i<listAppByRating.Count; i++)
                {
                    if (i == (int)BannerConstants.MAX)
                    {
                        break;
                    }
                    var slide = new SlideViewModel();
                    slide.Link = listAppByRating[i].ServiceAppModel.Banner;
                    slide.KeyId = Guid.Parse(listAppByRating[i].ServiceApplicationId+"");
                    slide.KeyType = CommonConstants.APP_IMAGE;
                    listSlide.Add(slide);
                }

            }

            var listEvent = await _eventService.GetListEventByPartyId(partyId, kiosk.Longtitude, kiosk.Latitude);
            if (listEvent.Count > (int)BannerConstants.NOT_MEET)
            {

                for (int i = 0; i < listEvent.Count; i++)
                {
                    if (i == (int)BannerConstants.MAX)
                    {
                        break;
                    }
                    var slide = new SlideViewModel();
                    slide.Link = listEvent[i].Banner;
                    slide.KeyId = listEvent[i].Id;
                    slide.KeyType = CommonConstants.EVENT_IMAGE;
                    listSlide.Add(slide);
                }
            }

            var listPoi = await _poiService.GetListPoiByPartyId(partyId, kiosk.Longtitude, kiosk.Latitude);
            if (listPoi.Count > (int)BannerConstants.NOT_MEET)
            {

                for(int i = 0; i < listPoi.Count; i++)
                {
                    if (i == (int)BannerConstants.MAX)
                    {
                        break;
                    }
                    var slide = new SlideViewModel();
                    slide.Link = listPoi[i].Banner;
                    slide.KeyId = listPoi[i].Id;
                    slide.KeyType = CommonConstants.POI_IMAGE;
                    listSlide.Add(slide);
                }
            }

            return listSlide;
        }
    }
}