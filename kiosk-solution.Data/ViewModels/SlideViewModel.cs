using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class SlideViewModel
    {
        public string Link { get; set; }
        public Guid KeyId { get; set; }
        public string KeyType { get; set; }
    }
}
