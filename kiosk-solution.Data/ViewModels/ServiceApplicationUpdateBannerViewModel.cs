using System;
using System.ComponentModel.DataAnnotations;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationUpdateBannerViewModel
    {
        [Required]
        public Guid ServiceApplicationId { get; set; }
        public string Banner { get; set; }
    }
}