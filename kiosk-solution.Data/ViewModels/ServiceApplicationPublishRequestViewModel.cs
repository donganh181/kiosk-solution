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
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        public Guid? HandlerId { get; set; }
        public string HandlerName { get; set; }
        public string HandlerEmail { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string ServiceApplicationName { get; set; }
        public string Status { get; set; }
        public string HandlerComment { get; set; }
    }
}
