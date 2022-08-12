using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class UpdateKioskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public Guid KioskLocationId { get; set; }
    }
}
