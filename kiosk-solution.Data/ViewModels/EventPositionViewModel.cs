using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class EventPositionViewModel
    {
        public Guid? TemplateId { get; set; }
        public string TemplateName { get; set; }
        public List<EventPositionDetailViewModel> ListPosition { get; set; }
    }

    public class EventPositionDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public string EventName { get; set; }
        public string TemplateName { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
    }
}