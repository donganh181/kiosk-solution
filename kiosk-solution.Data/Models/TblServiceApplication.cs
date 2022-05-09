using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblServiceApplication
    {
        public TblServiceApplication()
        {
            TblLocationOwnerServiceApplications = new HashSet<TblLocationOwnerServiceApplication>();
            TblOrders = new HashSet<TblOrder>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public Guid? ServiceProviderId { get; set; }
        public Guid? AppCategoryId { get; set; }
        public Guid? ApplicationMarketId { get; set; }
        public string Status { get; set; }

        public virtual TblAppCategory AppCategory { get; set; }
        public virtual TblApplicationMarket ApplicationMarket { get; set; }
        public virtual TblServiceProvider ServiceProvider { get; set; }
        public virtual ICollection<TblLocationOwnerServiceApplication> TblLocationOwnerServiceApplications { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
