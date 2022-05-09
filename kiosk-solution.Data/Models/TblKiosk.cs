using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblKiosk
    {
        public TblKiosk()
        {
            TblOrders = new HashSet<TblOrder>();
            TblSchedules = new HashSet<TblSchedule>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatDate { get; set; }
        public Guid? LocationOwnerId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public string Status { get; set; }

        public virtual TblKioskLocation KioskLocation { get; set; }
        public virtual TblLocationOwner LocationOwner { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblSchedule> TblSchedules { get; set; }
    }
}
