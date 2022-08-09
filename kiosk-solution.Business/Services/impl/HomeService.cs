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
        private readonly IPoiService _poiService;
        private readonly IEventService _eventService;

        public HomeService(ILogger<IHomeService> logger,
            IPartyServiceApplicationService appService,
            IPoiService poiService, IEventService eventService)
        {
            _logger = logger;
            _appService = appService;
            _poiService = poiService;
            _eventService = eventService;
        }

        public async Task<List<SlideViewModel>> GetListHomeImage(Guid partyId)
        {
            var listSlide = new List<SlideViewModel>();

            var listApp = await _appService.GetListAppByPartyId(partyId);

            if (listApp.Count >= 2)
            {
                for(int i = 0; i <= 1; i ++)
                {
                    var slide = new SlideViewModel();
                    slide.Link = listApp[i].ServiceAppModel.Banner;
                    slide.KeyId = listApp[i].Id;
                    slide.KeyType = CommonConstants.APP_IMAGE;
                    listSlide.Add(slide);
                }
            }

            var listEvent = await _eventService.GetListEventByPartyId(partyId);
            if (listEvent.Count >= 2)
            {
                for (int i = 0; i <= 1; i++)
                {
                    var slide = new SlideViewModel();
                    slide.Link = listEvent[i].Banner;
                    slide.KeyId = listEvent[i].Id;
                    slide.KeyType = CommonConstants.EVENT_IMAGE;
                    listSlide.Add(slide);
                }
            }

            var listPoi = await _poiService.GetListPoiByPartyId(partyId);
            if (listPoi.Count >= 2)
            {
                for (int i = 0; i <= 1; i++)
                {
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
