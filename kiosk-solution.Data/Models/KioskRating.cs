using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class KioskRating
    {
        public Guid Id { get; set; }
        public Guid? KioskId { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Kiosk Kiosk { get; set; }
    }
}
