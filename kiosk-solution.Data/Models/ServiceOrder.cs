using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ServiceOrder
    {
        public Guid Id { get; set; }
        public decimal? Total { get; set; }
        public DateTime? CreateDate { get; set; }
        public string OrderDetail { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public decimal? Commission { get; set; }
        public decimal? SystemCommission { get; set; }

        public virtual Kiosk Kiosk { get; set; }
        public virtual ServiceApplication ServiceApplication { get; set; }
    }
}
