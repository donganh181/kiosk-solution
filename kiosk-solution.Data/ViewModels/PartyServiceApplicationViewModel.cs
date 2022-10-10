using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PartyServiceApplicationViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [BindNever]
        public Guid? PartyId { get; set; }
        [BindNever]
        public string PartyName { get; set; }
        [BindNever]
        public string PartyEmail { get; set; }
        [BindNever]
        public Guid? ServiceApplicationId { get; set; }
        [BindNever]
        public string ServiceApplicationName { get; set; }
        [BindNever]
        public string ServiceApplicationDescription { get; set; }
        [BindNever]
        public string ServiceApplicationLogo { get; set; }
        [BindNever]
        public string ServiceApplicationLink { get; set; }
        [BindNever]
        public Guid? AppCategoryId { get; set; }
        [BindNever]
        public string AppcategoryName { get; set; }
        [BindNever]
        public string Status { get; set; }
    }
}
