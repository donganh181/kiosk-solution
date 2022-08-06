using System;
using System.Collections.Generic;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCommissionMonthViewModel
    {
        public List<string> Lables { get; set; }
        public List<decimal> Data { get; set; }
    }
}