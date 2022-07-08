using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using kiosk_solution.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class CreateScheduleViewModel
    {
        [Required] public string Name { get; set; }
        [Required] public string StringTimeStart { get; set; }
        [Required] public string StringTimeEnd { get; set; }
        [Required] public string DayOfWeek { get; set; }
    }
}