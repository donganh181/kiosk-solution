using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class TemplateCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
