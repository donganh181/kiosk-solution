using System;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace kiosk_solution.Business.SystemSchedule.Jobs
{
    public class CheckTemplateJob : IJob
    {
        private readonly ILogger<CheckTemplateJob> _logger;
        private readonly IKioskService _kioskService;
        private readonly INotiService _fcmService;

        public CheckTemplateJob(ILogger<CheckTemplateJob> logger, IKioskService kioskService,
            INotiService fcmService)
        {
            _logger = logger;
            _kioskService = kioskService;
            _fcmService = fcmService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            /*var listKiosk = await _kioskService.GetListSpecificKiosk();

            foreach(var item in listKiosk)
            {
                if(item.KioskScheduleTemplate == null)
                {
                    //send default
                    await _fcmService.SendNotificationToUser(item.DeviceId);
                    _logger.LogInformation($"Send notification to change default template to Kiosk {item.Id}");
                }
                else
                {
                    //send specific
                    await _fcmService.SendNotificationToChangeTemplate(item.KioskScheduleTemplate.Template, item.DeviceId);
                    _logger.LogInformation($"Send notification to change specific template to Kiosk {item.Id}");
                }
            }*/
            _logger.LogInformation("check template job running...");
            return;
        }
    }
}