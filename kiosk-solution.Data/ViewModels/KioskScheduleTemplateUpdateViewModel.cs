using System;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskScheduleTemplateUpdateViewModel
    {
        public Guid Id { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid TemplateId { get; set; }
    }
}