using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.ViewModels
{
    public class TokenViewModel
    {
        public TokenViewModel(Guid id, string role, string mail, string phoneNumber)
        {
            Id = id;
            Role = role;
            Mail = mail;
            PhoneNumber = phoneNumber;
        }
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
    }
}
