using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Party
    {
        public Party()
        {
            Events = new HashSet<Event>();
            InverseCreator = new HashSet<Party>();
            KioskLocations = new HashSet<KioskLocation>();
            Kiosks = new HashSet<Kiosk>();
            PartyNotifications = new HashSet<PartyNotification>();
            PartyServiceApplications = new HashSet<PartyServiceApplication>();
            Pois = new HashSet<Poi>();
            Schedules = new HashSet<Schedule>();
            ServiceApplicationFeedBacks = new HashSet<ServiceApplicationFeedBack>();
            ServiceApplicationPublishRequestCreators = new HashSet<ServiceApplicationPublishRequest>();
            ServiceApplicationPublishRequestHandlers = new HashSet<ServiceApplicationPublishRequest>();
            ServiceApplications = new HashSet<ServiceApplication>();
            Templates = new HashSet<Template>();
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
        public string DeviceId { get; set; }
        public string VerifyCode { get; set; }

        public virtual Party Creator { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Party> InverseCreator { get; set; }
        public virtual ICollection<KioskLocation> KioskLocations { get; set; }
        public virtual ICollection<Kiosk> Kiosks { get; set; }
        public virtual ICollection<PartyNotification> PartyNotifications { get; set; }
        public virtual ICollection<PartyServiceApplication> PartyServiceApplications { get; set; }
        public virtual ICollection<Poi> Pois { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<ServiceApplicationFeedBack> ServiceApplicationFeedBacks { get; set; }
        public virtual ICollection<ServiceApplicationPublishRequest> ServiceApplicationPublishRequestCreators { get; set; }
        public virtual ICollection<ServiceApplicationPublishRequest> ServiceApplicationPublishRequestHandlers { get; set; }
        public virtual ICollection<ServiceApplication> ServiceApplications { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
    }
}
