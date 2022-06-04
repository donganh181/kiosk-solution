﻿using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ServiceApplication
    {
        public ServiceApplication()
        {
            PartyServiceApplications = new HashSet<PartyServiceApplication>();
            ServiceApplicationPublishRequests = new HashSet<ServiceApplicationPublishRequest>();
            ServiceOrders = new HashSet<ServiceOrder>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? AppCategoryId { get; set; }
        public Guid? ApplicationMarketId { get; set; }
        public string Status { get; set; }

        public virtual AppCategory AppCategory { get; set; }
        public virtual ApplicationMarket ApplicationMarket { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<PartyServiceApplication> PartyServiceApplications { get; set; }
        public virtual ICollection<ServiceApplicationPublishRequest> ServiceApplicationPublishRequests { get; set; }
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
    }
}
