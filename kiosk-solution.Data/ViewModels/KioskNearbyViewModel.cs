using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskNearbyViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [BindNever]
        public Guid? PartyId { get; set; }
        [BindNever]
        public Guid? KioskLocationId { get; set; }
        [String]
        public string Status { get; set; }
        [Specific]
        public double Longtitude { get; set; }
        [Specific]
        public double Latitude { get; set; }
        [BindNever]
        public string DeviceId { get; set; }
    }
}
