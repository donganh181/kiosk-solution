using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class NotificationCreateViewModel
    {
        public Guid PartyId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
