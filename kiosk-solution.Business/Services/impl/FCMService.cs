using FCM.Net;
using kiosk_solution.Data.Constants;
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
            using (var sender = new Sender(FirebaseConstants.SERVER_KEY))
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
                if(result == null || !result.ReasonPhrase.Equals("OK"))
                {
                    _logger.LogInformation("Firebase error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase error.");
                }
                return true;
            }
        }
    }
}
