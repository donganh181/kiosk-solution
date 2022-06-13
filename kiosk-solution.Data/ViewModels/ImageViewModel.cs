using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ImageViewModel
    {
        [BindNever] public Guid Id { get; set; }
        [BindNever] public string Link { get; set; }
        [BindNever] public Guid? KeyId { get; set; }
        [BindNever] public string KeyType { get; set; }
    }
}
