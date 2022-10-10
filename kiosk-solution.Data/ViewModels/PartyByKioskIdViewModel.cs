using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PartyByKioskIdViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
        public string DeviceId { get; set; }
    }
}
