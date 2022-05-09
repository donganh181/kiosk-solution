using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Address { get; set; }
        public string TemplateId { get; set; }
        public Guid? CreatorId { get; set; }
        public string CreatorType { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
