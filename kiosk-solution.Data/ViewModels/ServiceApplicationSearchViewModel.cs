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
    public class ServiceApplicationSearchViewModel
    {
        [BindNever] public Guid? Id { get; set; }
        [String] public string Name { get; set; }
        [BindNever] public string Description { get; set; }
        [BindNever] public string Logo { get; set; }
        [BindNever] public string Link { get; set; }
        [BindNever] public Guid? PartyId { get; set; }
        [String] public string PartyName { get; set; }
        [String] public string PartyEmail { get; set; }
        [Guid] public Guid? AppCategoryId { get; set; }
        [String] public string AppCategoryName { get; set; }
        [BindNever] public PartyServiceApplicationViewModel PartyServiceApplication { get; set; }
        [String] public string Status { get; set; }
        [BindNever] public DateTime? CreateDate { get; set; }
        [BindNever] public double? AverageRating { get; set; }
        [BindNever] public int? NumberOfRating { get; set; }
        [BindNever] public List<ServiceApplicationFeedBackViewModel> ListFeedback { get; set; }
        [BindNever] public int? UserInstalled { get; set; }
        [BindNever] public string Banner { get; set; }
        [BindNever]public bool? IsAffiliate { get; set; }
    }
}