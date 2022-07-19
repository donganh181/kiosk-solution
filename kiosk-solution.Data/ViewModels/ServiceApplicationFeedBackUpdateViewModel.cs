using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationFeedBackUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
