using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCreateViewModel
    {
        public string OrderDetail { get; set; }
        public Guid KioskId { get; set; }
        public Guid ServiceApplicationId { get; set; }
    }
}