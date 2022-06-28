using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ImageReplaceViewModel
    {
        public Guid Id { get; set; }
        public List<Guid> RemoveFields { get; set; }
        public List<string> AddFields { get; set; }
    }
}
