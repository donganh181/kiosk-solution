using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class AppCategoryPositionSpecificViewModel
    {
        public Guid Id { get; set; }
        public Guid AppCategoryId { get; set; }
        public string AppCategoryName { get; set; }
        public string AppCategoryLogo { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
    }
}
