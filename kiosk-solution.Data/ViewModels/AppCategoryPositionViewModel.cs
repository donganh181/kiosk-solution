using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class AppCategoryPositionViewModel
    {
        public AppCategoryPositionViewModel(Guid? templateId, string templateName, List<CategoryPositionDetailViewModel> listPosition)
        {
            TemplateId = templateId;
            TemplateName = templateName;
            ListPosition = listPosition;
        }

        public Guid? TemplateId { get; set; }
        public string TemplateName { get; set; }
        public List<CategoryPositionDetailViewModel> ListPosition { get; set; }
    }
    public class CategoryPositionDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid? AppCategoryId { get; set; }
        public string AppCategoryName { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }

    }
}
