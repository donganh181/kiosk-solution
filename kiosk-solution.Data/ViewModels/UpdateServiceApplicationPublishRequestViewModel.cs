using System;

namespace kiosk_solution.Data.ViewModels
{
    public class UpdateServiceApplicationPublishRequestViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string HandlerComment { get; set; }
    }
}