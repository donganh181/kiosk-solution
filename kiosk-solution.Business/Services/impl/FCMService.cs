using FCM.Net;
using kiosk_solution.Data.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services.impl
{
    public class FCMService : INotiService
    {
        private readonly ILogger<INotiService> _logger;

        public FCMService(ILogger<INotiService> logger)
        {
            _logger = logger;
        }
        public async Task<bool> SendNotificationToUser(string deviceId)
        {
            using (var sender = new Sender("AAAAz10Ppsw:APA91bEdkD0Byh3nc641p6TQizr4FcVpzbvAHa6vcSL0keSKFiF5FFmusHfQtUTdoqoFHybK5VQP9qlnH_-eU2C6_9QUNi9_SZOnPu7Diz-VLZNQ4KebxSybAkLmMpYtPm7NoHCdcxaD"))
            {
                var message = new Message
                {
                    RegistrationIds = new List<string> { deviceId },
                    Notification = new Notification
                    {
                        Title = "Test!",
                        Body = "Check it now"
                    }
                };
                var result = await sender.SendAsync(message);
                if(result == null)
                {
                    _logger.LogInformation("Firebase error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase error.");
                }
                return true;
            }
        }
    }
}
