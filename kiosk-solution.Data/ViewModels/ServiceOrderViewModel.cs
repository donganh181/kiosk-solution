using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderViewModel
    {
        public Guid Id { get; set; }
        public decimal? Income { get; set; }
        public DateTime? CreateDate { get; set; }
        public string OrderDetail { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
    }
}