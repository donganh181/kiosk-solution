using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblLocationOwnerServiceApplication
    {
        public Guid Id { get; set; }
        public Guid? LocationOwnerId { get; set; }
        public Guid? ServiceApplicationId { get; set; }

        public virtual TblLocationOwner LocationOwner { get; set; }
        public virtual TblServiceApplication ServiceApplication { get; set; }
    }
}
