using System;
using System.ComponentModel.DataAnnotations;

namespace kiosk_solution.Data.ViewModels
{
    public class UpdatePasswordViewModel
    {
        [Required] public string NewPassword { get; set; }
        [Required] public string OldPassword { get; set; }
    }
}