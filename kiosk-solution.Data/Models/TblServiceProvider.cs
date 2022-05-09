using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblServiceProvider
    {
        public TblServiceProvider()
        {
            TblOrders = new HashSet<TblOrder>();
            TblServiceApplications = new HashSet<TblServiceApplication>();
        }

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

        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblServiceApplication> TblServiceApplications { get; set; }
    }
}
