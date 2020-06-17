using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class City
    {
        public City()
        {
            Countries = new HashSet<Country>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}
