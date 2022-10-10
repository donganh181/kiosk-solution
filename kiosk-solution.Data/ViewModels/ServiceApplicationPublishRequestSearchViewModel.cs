using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceApplicationPublishRequestSearchViewModel
    {
        [BindNever] public Guid? Id { get; set; }
        [BindNever] public Guid? CreatorId { get; set; }
        [String] public string CreatorName { get; set; }
        [String] public string CreatorEmail { get; set; }
        [BindNever] public Guid? HandlerId { get; set; }

        [String] public string HandlerName { get; set; }
        [String] public string HandlerEmail { get; set; }
        [BindNever] public Guid? ServiceApplicationId { get; set; }
        [String] public string ServiceApplicationName { get; set; }
        [String] public string Status { get; set; }
        [BindNever] public string HandlerComment { get; set; }
        [BindNever] public DateTime? CreateDate { get; set; }
    }
}