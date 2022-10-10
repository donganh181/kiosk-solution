using System;
using System.Collections.Generic;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class EventSearchViewModel
    {
        [BindNever] public Guid? Id { get; set; }
        [String] public string Name { get; set; }
        [BindNever] public ImageViewModel Thumbnail { get; set; }
        [BindNever] public string Description { get; set; }
        [BindNever] public DateTime? TimeStart { get; set; }
        [BindNever] public DateTime? TimeEnd { get; set; }
        [BindNever] public string Longtitude { get; set; }
        [BindNever] public string Latitude { get; set; }
        [String] public string Street { get; set; }
        [String] public string Ward { get; set; }
        [String] public string District { get; set; }
        [String] public string City { get; set; }
        [BindNever] public string Address { get; set; }
        [BindNever] public Guid? CreatorId { get; set; }
        [String] public string CreatorName { get; set; }
        [String] public string CreatorEmail { get; set; }
        [String] public string Type { get; set; }
        [String] public string Status { get; set; }
        [BindNever] public string Banner { get; set; }
        [BindNever] public DateTime? CreateDate { get; set; }
        [BindNever] public List<EventImageDetailViewModel> ListImage { get; set; }
    }
}