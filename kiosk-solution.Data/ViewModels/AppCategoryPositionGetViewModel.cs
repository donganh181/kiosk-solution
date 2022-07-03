using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class AppCategoryPositionGetViewModel
    {
        public AppCategoryPositionGetViewModel(Guid templateId, string templateName, List<AppCategoryPositionByRowViewModel> listPosition)
        {
            TemplateId = templateId;
            TemplateName = templateName;
            ListPosition = listPosition;
        }

        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public List<AppCategoryPositionByRowViewModel> ListPosition { get; set; }
    }

    public class AppCategoryPositionByRowViewModel
    {
        public AppCategoryPositionByRowViewModel(int rowIndex, List<AppCategoryPositionDetailViewModel> components)
        {
            RowIndex = rowIndex;
            Components = components;
        }

        public int RowIndex { get; set; }
        public List<AppCategoryPositionDetailViewModel> Components { get; set; }
    }
    public class AppCategoryPositionDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid AppCategoryId { get; set; }
        public string AppCategoryName { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        
    }
}