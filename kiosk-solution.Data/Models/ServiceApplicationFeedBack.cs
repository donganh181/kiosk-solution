using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ServiceApplicationFeedBack
    {
        public Guid Id { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public Guid? PartyId { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Party Party { get; set; }
        public virtual ServiceApplication ServiceApplication { get; set; }
    }
}
