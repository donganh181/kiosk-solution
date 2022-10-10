using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface INotificationService
    {
        public Task<NotificationViewModel> Create(NotificationCreateViewModel model);
        public Task<NotificationViewModel> Get(Guid id);
    }
}
