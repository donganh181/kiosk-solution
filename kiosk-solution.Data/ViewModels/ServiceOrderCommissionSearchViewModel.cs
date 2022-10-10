using System;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCommissionSearchViewModel
    {
        [Guid]
        public Guid? ServiceApplicationId { get; set; }
        [BindNever]
        public string ServiceApplicationName { get; set; }
        [BindNever]
        public Guid? KioskId { get; set; }
        [BindNever]
        public string KioskName { get; set; }
        [BindNever]
        [Skip]
        public double TotalCommission { get; set; }
    }
}