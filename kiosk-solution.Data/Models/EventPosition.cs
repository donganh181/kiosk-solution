using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class EventPosition
    {
        public Guid Id { get; set; }
        public Guid? TemplateId { get; set; }
        public Guid? EventId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public string Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual Template Template { get; set; }
    }
}
