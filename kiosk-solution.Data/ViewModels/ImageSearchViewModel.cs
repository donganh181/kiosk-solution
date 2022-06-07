using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ImageSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [BindNever]
        public string Link { get; set; }
        [Guid]
        public Guid? KeyId { get; set; }
        [String]
        public string KeyType { get; set; }
    }
}
