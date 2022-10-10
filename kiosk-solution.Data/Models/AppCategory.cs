using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class AppCategory
    {
        public AppCategory()
        {
            AppCategoryPositions = new HashSet<AppCategoryPosition>();
            ServiceApplications = new HashSet<ServiceApplication>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public double? CommissionPercentage { get; set; }

        public virtual ICollection<AppCategoryPosition> AppCategoryPositions { get; set; }
        public virtual ICollection<ServiceApplication> ServiceApplications { get; set; }
    }
}
