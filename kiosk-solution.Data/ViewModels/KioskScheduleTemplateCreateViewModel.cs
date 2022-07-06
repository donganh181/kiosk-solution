using System;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskScheduleTemplateCreateViewModel
    {
        public Guid kioskId { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid TemplateId { get; set; }
    }
}