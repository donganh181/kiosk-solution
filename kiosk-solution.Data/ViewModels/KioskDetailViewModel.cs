using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskDetailViewModel
    {

        //Kiosk zone
        public Guid KioskId { get; set; }
        public string DeviceId { get; set; }
        public List<KioskScheduleTemplateViewModel> KioskScheduleTemplate { get; set; }

    }
}
