using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblParty
    {
        public TblParty()
        {
            InverseCreator = new HashSet<TblParty>();
            TblEvents = new HashSet<TblEvent>();
            TblKiosks = new HashSet<TblKiosk>();
            TblOrders = new HashSet<TblOrder>();
            TblPartyKioskLocations = new HashSet<TblPartyKioskLocation>();
            TblPartyServiceApplications = new HashSet<TblPartyServiceApplication>();
            TblPois = new HashSet<TblPoi>();
            TblSchedules = new HashSet<TblSchedule>();
            TblServiceApplications = new HashSet<TblServiceApplication>();
            TblTemplates = new HashSet<TblTemplate>();
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
        public Guid? RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual TblParty Creator { get; set; }
        public virtual TblRole Role { get; set; }
        public virtual ICollection<TblParty> InverseCreator { get; set; }
        public virtual ICollection<TblEvent> TblEvents { get; set; }
        public virtual ICollection<TblKiosk> TblKiosks { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblPartyKioskLocation> TblPartyKioskLocations { get; set; }
        public virtual ICollection<TblPartyServiceApplication> TblPartyServiceApplications { get; set; }
        public virtual ICollection<TblPoi> TblPois { get; set; }
        public virtual ICollection<TblSchedule> TblSchedules { get; set; }
        public virtual ICollection<TblServiceApplication> TblServiceApplications { get; set; }
        public virtual ICollection<TblTemplate> TblTemplates { get; set; }
    }
}
