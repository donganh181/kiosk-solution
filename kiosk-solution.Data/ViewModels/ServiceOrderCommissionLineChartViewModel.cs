using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCommissionLineChartViewModel
    {
        public List<AppDataViewModel> Datas { get; set; }
    }

    public class AppDataViewModel
    {
        public Guid ServiceApplicationId { get; set; }
        public string ServiceApplicationName { get; set; }
        public List<decimal> Datasets { get; set; }
    }
}