using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationCommissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double CommissionPercentage { get; set; }
    }
}