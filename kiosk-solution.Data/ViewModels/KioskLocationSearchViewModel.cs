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
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [BindNever]
        public string Status { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever]
        public string Description { get; set; }
        [BindNever]
        public Guid? OwnerId { get; set; }
        [BindNever]
        public string OwnerEmail { get; set; }
        [BindNever]
        public string HotLine { get; set; }
        [BindNever]
        public List<ImageViewModel> ListImage { get; set; }
    }
}
