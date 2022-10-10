using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class PartyServiceApplication
    {
        public Guid Id { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string Status { get; set; }

        public virtual Party Party { get; set; }
        public virtual ServiceApplication ServiceApplication { get; set; }
    }
}
