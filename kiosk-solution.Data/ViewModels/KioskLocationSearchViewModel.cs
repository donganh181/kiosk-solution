using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskLocationSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        public string Description { get; set; }
        [String]
        public string Street { get; set; }
        [String]
        public string Ward { get; set; }
        [String]
        public string District { get; set; }
        [String]
        public string Province { get; set; }
        [String]
        public string City { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [String]
        public string Status { get; set; }
    }
}
