using System;

namespace kiosk_solution.Data.ViewModels
{
    public class CreateScheduleViewModel
    {
        public string Name { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string StringTimeStart { get; set; }
        public string StringTimeEnd { get; set; }
        public string DayOfWeek { get; set; }
    }
}                       