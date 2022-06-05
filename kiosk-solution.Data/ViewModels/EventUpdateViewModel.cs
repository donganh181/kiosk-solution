using System;

namespace kiosk_solution.Data.ViewModels
{
    public class EventUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Address { get; set; }
    }
}