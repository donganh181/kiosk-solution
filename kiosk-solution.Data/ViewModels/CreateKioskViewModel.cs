using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class CreateKioskViewModel
    {
        [Required] public string Name { get; set; }
        [Required] public Guid? PartyId { get; set; }
    }
}
