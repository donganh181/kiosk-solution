using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class UpdateKioskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
    }
}
