using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class MyAppViewModel
    {
        public Guid Id { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string Status { get; set; }
        public ServiceApplicationViewModel ServiceAppModel { get; set; }
    }
}
