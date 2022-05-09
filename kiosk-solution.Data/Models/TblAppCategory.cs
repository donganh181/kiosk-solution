using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblAppCategory
    {
        public TblAppCategory()
        {
            TblServiceApplications = new HashSet<TblServiceApplication>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TblServiceApplication> TblServiceApplications { get; set; }
    }
}
