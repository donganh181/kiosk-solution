using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblServiceApplication
    {
        public TblServiceApplication()
        {
            TblOrders = new HashSet<TblOrder>();
            TblPartyServiceApplications = new HashSet<TblPartyServiceApplication>();
            TblPositions = new HashSet<TblPosition>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? AppCategoryId { get; set; }
        public Guid? ApplicationMarketId { get; set; }
        public string Status { get; set; }

        public virtual TblAppCategory AppCategory { get; set; }
        public virtual TblApplicationMarket ApplicationMarket { get; set; }
        public virtual TblParty Party { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblPartyServiceApplication> TblPartyServiceApplications { get; set; }
        public virtual ICollection<TblPosition> TblPositions { get; set; }
    }
}
