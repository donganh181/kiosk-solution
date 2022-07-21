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
        }

        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OwnerId { get; set; }
        public string HotLine { get; set; }

        public virtual Party Owner { get; set; }
        public virtual ICollection<Kiosk> Kiosks { get; set; }
    }
}
