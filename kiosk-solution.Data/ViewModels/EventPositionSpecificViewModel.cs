using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class EventPositionSpecificViewModel
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public string Status { get; set; }
    }
}
