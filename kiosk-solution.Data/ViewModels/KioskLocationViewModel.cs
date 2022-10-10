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
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OwnerId { get; set; }
        public string OwnerEmail { get; set; }
        public string HotLine { get; set; }
        public List<ImageViewModel> ListImage { get; set; }
    }
}
