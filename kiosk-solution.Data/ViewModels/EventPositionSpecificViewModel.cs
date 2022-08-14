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
        public Guid EventId { get; set; }
        
        public string EventName { get; set; }
        public ImageViewModel EventThumbnail { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string EventStatus { get; set; }
    }
}
