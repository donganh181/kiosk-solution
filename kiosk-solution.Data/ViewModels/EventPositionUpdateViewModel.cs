using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class EventPositionUpdateViewModel
    {
        public Guid? TemplateId { get; set; }
        public List<EventPositionDetailUpdateViewModel> ListPosition { get; set; }
    }
    public class EventPositionDetailUpdateViewModel
    {
        public Guid? EventId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
    }
}