using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblPoi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? OpenTime { get; set; }
        public string DayOfWeek { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
    }
}
