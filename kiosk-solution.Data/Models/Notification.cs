using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Notification
    {
        public Notification()
        {
            PartyNotifications = new HashSet<PartyNotification>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<PartyNotification> PartyNotifications { get; set; }
    }
}
