using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Order
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

        public virtual Kiosk Kiosk { get; set; }
        public virtual Party Party { get; set; }
        public virtual ServiceApplication ServiceApplication { get; set; }
    }
}
