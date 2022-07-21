using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class CreateKioskLocationViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HotLine { get; set; }
        public List<string> ListImage { get; set; }
    }
}
