using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationSpecificViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public Guid? PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyEmail { get; set; }
        public Guid? AppCategoryId { get; set; }
        public string AppCategoryName { get; set; }
        public string Status { get; set; }
        public double? AverageRating { get; set; }
        public int? NumberOfRating { get; set; }
        public List<ServiceApplicationFeedBackViewModel> ListFeedback { get; set; }
        public ServiceApplicationFeedBackViewModel MyFeedback { get; set; }
        public int? UserInstalled { get; set; }
        public bool? IsAffiliate { get; set; }
        public string Banner { get; set; }
    }
}
