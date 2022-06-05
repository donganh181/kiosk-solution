﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class EventCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime? TimeStart { get; set; }
        [Required]
        public DateTime? TimeEnd { get; set; }
        [Required]
        public string Address { get; set; }
    }
}