using System;
using System.Collections.Generic;

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
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public string DeviceId { get; set; }
        public List<KioskRatingViewModel> ListFeedback { get; set; }
        public double? AverageRating { get; set; }
        public int? NumberOfRating { get; set; }
    }
}