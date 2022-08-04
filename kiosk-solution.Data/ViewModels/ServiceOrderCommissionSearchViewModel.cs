using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCommissionSearchViewModel
    {
        public Guid serviceApplicationId { get; set; }
        public string serviceApplicationName { get; set; }
        public double totalCommission { get; set; }
    }
}