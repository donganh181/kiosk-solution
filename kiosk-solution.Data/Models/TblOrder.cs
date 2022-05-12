using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblOrder
    {
        public Guid Id { get; set; }
        public decimal? Income { get; set; }
        public DateTime? CreateDate { get; set; }
        public string OrderDetail { get; set; }
        public DateTime? SubmitDate { get; set; }
        public double? CommissionPercentage { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string Status { get; set; }

        public virtual TblKiosk Kiosk { get; set; }
        public virtual TblParty Party { get; set; }
        public virtual TblServiceApplication ServiceApplication { get; set; }
    }
}
