using System;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class EventSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever]
        public string Description { get; set; }
        [BindNever]
        public DateTime? TimeStart { get; set; }
        [BindNever]
        public DateTime? TimeEnd { get; set; }
        [BindNever]
        public string Address { get; set; }
        [Guid]
        public Guid? CreatorId { get; set; }
        [String]
        public string Type { get; set; }
        [String]
        public string Status { get; set; }
    }
}