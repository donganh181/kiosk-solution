using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class PartyKioskLocation
    {
        public Guid Id { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual KioskLocation KioskLocation { get; set; }
        public virtual Party Party { get; set; }
    }
}
