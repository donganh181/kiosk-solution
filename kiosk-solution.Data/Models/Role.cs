using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            Parties = new HashSet<Party>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Party> Parties { get; set; }
    }
}
