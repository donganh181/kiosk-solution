using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.ViewModels
{
    public class PartyServiceApplicationSearchViewModel
    {
        [BindNever] public Guid? Id { get; set; }
        [BindNever] public Guid? PartyId { get; set; }
        [BindNever] public string PartyName { get; set; }
        [BindNever] public string PartyEmail { get; set; }
        [Guid] public Guid? ServiceApplicationId { get; set; }
        [String] public string ServiceApplicationName { get; set; }
        [BindNever] public string ServiceApplicationDescription { get; set; }
        [BindNever] public string ServiceApplicationLogo { get; set; }
        [BindNever] public string ServiceApplicationLink { get; set; }
        [Guid] public Guid? AppCategoryId { get; set; }
        [String] public string AppcategoryName { get; set; }
        [String] public string Status { get; set; }

        [String] public string ServiceApplicationStatus { get; set; }
    }
}