using FCM.Net;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task<bool> SendNotification(NotificationCreateViewModel model, string deviceId)
        {
            string jsonConvert = JsonConvert.SerializeObject(model);

            using (var sender = new Sender(FirebaseConstants.SERVER_KEY))
            {
                var message = new Message
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"json", jsonConvert }
                    },

                    RegistrationIds = new List<string> { deviceId },
                    Notification = new Notification
                    {
                        Title = model.Title,
                        Body = "Check it now"
                    }
                };
                var result = await sender.SendAsync(message);
                if (result == null || !result.ReasonPhrase.Equals("OK"))
                {
                    _logger.LogInformation("Firebase error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase error.");
                }
                return true;
            }
        }

        public async Task<bool> SendNotificationToChangeTemplate(TemplateDetailViewModel model, string deviceId)
        {
            string jsonConvert = JsonConvert.SerializeObject(model);

            using (var sender = new Sender(FirebaseConstants.SERVER_KEY))
            {
                var message = new Message
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"json", jsonConvert }
                    },

                    RegistrationIds = new List<string> { deviceId },
                    Notification = new Notification
                    {
                        Title = "Change Template",
                        Body = "Change Template base on Schedule"
                    }
                };
                var result = await sender.SendAsync(message);
                if (result == null || !result.ReasonPhrase.Equals("OK"))
                {
                    _logger.LogInformation("Firebase error.");
                    throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase error.");
                }
                return true;
            }
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
                        Title = "Change default template!",
                        Body = "Default template"
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
