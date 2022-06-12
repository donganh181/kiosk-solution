using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class AppCategoryPositionCreateViewModel
    {
        public Guid? TemplateId { get; set; }
        public List<CategoryPositionDetailCreateViewModel> ListPosition {get;set;}
    }
    public class CategoryPositionDetailCreateViewModel
    {
        public Guid? AppCategoryId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
    }
}
