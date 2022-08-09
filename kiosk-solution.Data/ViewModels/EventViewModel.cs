using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public ImageViewModel Thumbnail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Guid? CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<EventImageDetailViewModel> ListImage { get; set; }
        public string Banner { get; set; }
    }
}
