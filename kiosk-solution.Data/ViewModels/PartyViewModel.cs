using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PartyViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public Guid? CreatorId { get; set; }
        public string CreatorMail { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public string RoleName { get; set; }
        public string DeviceId { get; set; }
        public bool PasswordIsChanged { get; set; }
    }
}
