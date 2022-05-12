using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblPosition
    {
        public Guid Id { get; set; }
        public Guid? TemplateId { get; set; }
        public string Description { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual TblServiceApplication ServiceApplication { get; set; }
        public virtual TblTemplate Template { get; set; }
    }
}
