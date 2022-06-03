using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationPublishRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? HandlerId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string Status { get; set; }
        public string HandlerComment { get; set; }
    }
}
