using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Image
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public Guid? KeyId { get; set; }
        public string KeyType { get; set; }
    }
}
