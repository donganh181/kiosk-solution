using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class AppCategoryPositionUpdateViewModel
    {
        public Guid? Id { get; set; }
        public Guid? TemplateId { get; set; }
        public Guid? AppCategoryId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
    }
}
