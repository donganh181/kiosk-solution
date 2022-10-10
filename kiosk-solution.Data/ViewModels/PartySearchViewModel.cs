using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PartySearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string FirstName { get; set; }
        [String]
        public string LastName { get; set; }
        [String]
        public string PhoneNumber { get; set; }
        [String]
        public string Email { get; set; }
        [String]
        public string Address { get; set; }
        [BindNever]
        public DateTime? DateOfBirth { get; set; }
        [BindNever]
        public Guid? CreatorId { get; set; }
        [String]
        public string CreatorMail { get; set; }
        [BindNever]
        public Guid? RoleId { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [String]
        public string Status { get; set; }
        [String]
        public string RoleName { get; set; }
    }
}
