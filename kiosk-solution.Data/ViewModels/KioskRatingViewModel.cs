using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskRatingViewModel
    {
        public Guid Id { get; set; }
        public Guid? KioskId { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
