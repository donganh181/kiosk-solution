using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class EventPositionGetViewModel
    {
        public EventPositionGetViewModel(Guid templateId, string templateName, List<EventPositionByRowViewModel> listPosition)
        {
            TemplateId = templateId;
            TemplateName = templateName;
            ListPosition = listPosition;
        }

        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public List<EventPositionByRowViewModel> ListPosition { get; set; }
    }

    public class EventPositionByRowViewModel
    {
        public EventPositionByRowViewModel(int rowIndex, List<EventPositionDetailGetViewModel> components)
        {
            RowIndex = rowIndex;
            Components = components;
        }

        public int RowIndex { get; set; }
        public List<EventPositionDetailGetViewModel> Components { get; set; }
    }
    public class EventPositionDetailGetViewModel
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string Status { get; set; }
    }
}