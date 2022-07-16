using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PartyNotificationViewModel
    {
        public Guid Id { get; set; }
        public Guid? PartyId { get; set; }
        public string PartyMail { get; set; }
        public Guid? NotificationId { get; set; }
        public string NotiTitle { get; set; }
        public string NotiContent { get; set; }
        public DateTime NotiCreateDate { get; set; }
        public string Status { get; set; }
    }
}
