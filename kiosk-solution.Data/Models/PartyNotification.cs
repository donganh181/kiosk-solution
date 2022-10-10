using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class PartyNotification
    {
        public Guid Id { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? NotificationId { get; set; }
        public string Status { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual Party Party { get; set; }
    }
}
