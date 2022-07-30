using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskRatingCreateViewModel
    {
        public Guid? KioskId { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; }
    }
}
