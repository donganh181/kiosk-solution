using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiImageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PoiImageDetailViewModel> ListImage { get; set; }
    }
    public class PoiImageDetailViewModel
    {
        [BindNever] public Guid Id { get; set; }
        [BindNever] public string Link { get; set; }
    }
}
