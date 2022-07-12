using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class TemplateDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<AppCategoryPositionSpecificViewModel> ListAppCatePosition { get; set; }
        public List<EventPositionSpecificViewModel> ListEventPosition { get; set; }
    }
}
