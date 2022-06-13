﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class EventImageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EventImageDetailViewModel> ListImage { get; set; }
    }
    public class EventImageDetailViewModel
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
    }
}
