using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Poicategory
    {
        public Poicategory()
        {
            Pois = new HashSet<Poi>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<Poi> Pois { get; set; }
    }
}
