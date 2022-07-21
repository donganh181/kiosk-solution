using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class UpdateKioskLocationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HotLine { get; set; }
    }
}
