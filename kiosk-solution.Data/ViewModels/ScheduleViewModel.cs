using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ScheduleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public string StringTimeStart { get; set; }
        public string StringTimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public Guid? PartyId { get; set; }
        public string Status { get; set; }
    }
}