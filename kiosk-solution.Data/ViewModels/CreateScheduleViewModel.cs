using System;

namespace kiosk_solution.Data.ViewModels
{
    public class CreateScheduleViewModel
    {
        public string Name { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string DayOfWeek { get; set; }
    }
}