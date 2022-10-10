using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderLocationOwnerViewModel
    {
        public Guid Id { get; set; }
        public decimal? Total { get; set; }
        public decimal? Commission { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? KioskId { get; set; }
        public string KioskName { get; set; }
        public Guid? ServiceApplicationId { get; set; }
    }
}