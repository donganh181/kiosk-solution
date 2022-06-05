using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PartyServiceApplicationViewModel
    {
        public Guid Id { get; set; }
        public Guid? PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyEmail { get; set; }
        public Guid? ServiceApplicationId { get; set; }
        public string ServiceApplicationName { get; set; }
    }
}
