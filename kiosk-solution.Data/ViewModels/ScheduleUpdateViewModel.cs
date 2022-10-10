using System;

namespace kiosk_solution.Data.ViewModels
{
    public class ScheduleUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StringTimeStart { get; set; }
        public string StringTimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public string Status { get; set; } 
    }
}