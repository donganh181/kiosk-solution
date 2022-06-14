using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiAddImageViewModel
    {
        public Guid Id { get; set; }
        public List<PoiImageDetailCreateViewModel> ListImage { get; set; }
    }
    public class PoiImageDetailCreateViewModel
    {
        public string Image { get; set; }
    }
}
