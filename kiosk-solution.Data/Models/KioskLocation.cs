using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class KioskLocation
    {
        public KioskLocation()
        {
            Kiosks = new HashSet<Kiosk>();
            PartyKioskLocations = new HashSet<PartyKioskLocation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Kiosk> Kiosks { get; set; }
        public virtual ICollection<PartyKioskLocation> PartyKioskLocations { get; set; }
    }
}
