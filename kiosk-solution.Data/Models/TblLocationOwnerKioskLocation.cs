using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblLocationOwnerKioskLocation
    {
        public Guid Id { get; set; }
        public Guid? LocationOwnerId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual TblKioskLocation KioskLocation { get; set; }
        public virtual TblLocationOwner LocationOwner { get; set; }
    }
}
