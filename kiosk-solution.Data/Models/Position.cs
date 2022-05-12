using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Position
    {
        public Guid Id { get; set; }
        public Guid? TemplateId { get; set; }
        public string Description { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual ServiceApplication ServiceApplication { get; set; }
        public virtual Template Template { get; set; }
    }
}
