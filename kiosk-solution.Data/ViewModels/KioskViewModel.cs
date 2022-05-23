using System;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
    }
}