using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ApplicationMarket
    {
        public ApplicationMarket()
        {
            ServiceApplications = new HashSet<ServiceApplication>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ServiceApplication> ServiceApplications { get; set; }
    }
}
