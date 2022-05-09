using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceProviderViewModel
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
        public string CreatorType { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
    }
}
