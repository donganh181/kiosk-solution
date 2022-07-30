using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [Guid]
        public Guid? PartyId { get; set; }
        [BindNever]
        public Guid? KioskLocationId { get; set; }
        [String]
        public string KioskLocationName { get; set; }
        [String]
        public string Status { get; set; }
        [String]
        public string Longtitude { get; set; }
        [String]
        public string Latitude { get; set; }
        [BindNever]
        public string DeviceId { get; set; }
        [BindNever] public List<KioskRatingViewModel> ListFeedback { get; set; }
        [BindNever] public double? AverageRating { get; set; }
        [BindNever] public int? NumberOfRating { get; set; }
    }
}
