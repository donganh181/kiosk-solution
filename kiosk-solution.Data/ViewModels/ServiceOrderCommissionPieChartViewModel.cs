using System;
using System.Collections.Generic;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCommissionPieChartViewModel
    {
        public List<string> Labels { get; set; }
        public List<decimal> Datasets { get; set; }
    }
}