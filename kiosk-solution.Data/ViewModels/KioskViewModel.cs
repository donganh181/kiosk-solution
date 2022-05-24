using System;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public string Status { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
    }
}