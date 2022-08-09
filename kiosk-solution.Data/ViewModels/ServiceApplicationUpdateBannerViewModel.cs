using System;
using System.ComponentModel.DataAnnotations;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationUpdateBannerViewModel
    {
        [Required]
        public Guid ServiceApplicationId { get; set; }
        [Required]
        public string Banner { get; set; }
    }
}