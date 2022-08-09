using System;
using System.ComponentModel.DataAnnotations;

namespace kiosk_solution.Data.ViewModels
{
    public class EventUpdateBannerViewModel
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string Banner { get; set; }
    }
}