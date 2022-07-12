using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface INotiService
    {
        public Task<bool> SendNotificationToUser(string deviceId);
        public Task<bool> SendNotification(NotificationCreateViewModel model, string deviceId);
        public Task<bool> SendNotificationToChangeTemplate(TemplateDetailViewModel model, string deviceId);
    }
}
