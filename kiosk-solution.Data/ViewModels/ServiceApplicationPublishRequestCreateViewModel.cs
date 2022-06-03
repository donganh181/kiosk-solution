using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationPublishRequestCreateViewModel
    {
        public Guid? CreatorId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
    }
}
