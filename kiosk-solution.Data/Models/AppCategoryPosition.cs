using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class AppCategoryPosition
    {
        public Guid Id { get; set; }
        public Guid? TemplateId { get; set; }
        public Guid? AppCategoryId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }

        public virtual AppCategory AppCategory { get; set; }
        public virtual Template Template { get; set; }
    }
}
