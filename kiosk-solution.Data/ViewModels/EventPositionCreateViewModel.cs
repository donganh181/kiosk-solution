using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class EventPositionCreateViewModel
    {
        public Guid? TemplateId { get; set; }
        public List<EventPositionDetailCreateViewModel> ListPosition {get;set;}
    }
    public class EventPositionDetailCreateViewModel
    {
        public Guid? EventId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
    }
}