using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class AppCategory
    {
        public AppCategory()
        {
            ServiceApplications = new HashSet<ServiceApplication>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ServiceApplication> ServiceApplications { get; set; }
    }
}
