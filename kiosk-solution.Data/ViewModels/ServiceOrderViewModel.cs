using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderViewModel
    {
        public Guid Id { get; set; }
        public decimal? Total { get; set; }
        public decimal? Commission { get; set; }
        public decimal? SystemCommission { get; set; }
        public DateTime? CreateDate { get; set; }
        public string OrderDetail { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string KioskName { get; set; }
    }
}