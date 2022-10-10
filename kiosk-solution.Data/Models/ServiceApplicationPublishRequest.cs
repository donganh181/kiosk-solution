using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ServiceApplicationPublishRequest
    {
        public Guid Id { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? HandlerId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string Status { get; set; }
        public string HandlerComment { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Party Creator { get; set; }
        public virtual Party Handler { get; set; }
        public virtual ServiceApplication ServiceApplication { get; set; }
    }
}
