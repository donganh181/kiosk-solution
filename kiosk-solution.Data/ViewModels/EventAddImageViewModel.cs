using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class EventAddImageViewModel
    {
        public Guid Id { get; set; }
        public List<EventImageDetailCreateViewModel> ListImage { get; set; }
    }
    public class EventImageDetailCreateViewModel
    {
        public string Image { get; set; }
    }
}
