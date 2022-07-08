using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ScheduleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public Guid? PartyId { get; set; }
        public string Status { get; set; }
    }
}