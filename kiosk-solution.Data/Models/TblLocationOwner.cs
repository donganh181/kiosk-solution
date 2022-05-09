using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblLocationOwner
    {
        public TblLocationOwner()
        {
            TblKiosks = new HashSet<TblKiosk>();
            TblLocationOwnerKioskLocations = new HashSet<TblLocationOwnerKioskLocation>();
            TblLocationOwnerServiceApplications = new HashSet<TblLocationOwnerServiceApplication>();
            TblPositions = new HashSet<TblPosition>();
            TblSchedules = new HashSet<TblSchedule>();
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
        public string CreatorId { get; set; }
        public string CreatetorType { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<TblKiosk> TblKiosks { get; set; }
        public virtual ICollection<TblLocationOwnerKioskLocation> TblLocationOwnerKioskLocations { get; set; }
        public virtual ICollection<TblLocationOwnerServiceApplication> TblLocationOwnerServiceApplications { get; set; }
        public virtual ICollection<TblPosition> TblPositions { get; set; }
        public virtual ICollection<TblSchedule> TblSchedules { get; set; }
        public virtual ICollection<TblTemplate> TblTemplates { get; set; }
    }
}
