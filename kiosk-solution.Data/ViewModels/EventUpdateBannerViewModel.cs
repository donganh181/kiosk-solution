using System;
using System.ComponentModel.DataAnnotations;

namespace kiosk_solution.Data.ViewModels
{
    public class EventUpdateBannerViewModel
    {
        [Required]
        public Guid EventId { get; set; }
        public string Banner { get; set; }
    }
}