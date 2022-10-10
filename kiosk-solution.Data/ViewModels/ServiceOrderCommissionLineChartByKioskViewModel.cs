using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderCommissionLineChartByKioskViewModel
    {
        public List<KioskDataViewModel> Datas { get; set; }
    }
    
    public class KioskDataViewModel
    {
        public Guid KioskId { get; set; }
        public string KioskName { get; set; }
        public List<decimal> Datasets { get; set; }
    }
}