using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskLocationViewModel
    {
        public Guid Id { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
    }
}
