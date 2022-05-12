using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblParties = new HashSet<TblParty>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TblParty> TblParties { get; set; }
    }
}
