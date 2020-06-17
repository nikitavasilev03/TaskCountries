using System;
using System.Collections.Generic;

namespace DomainCore.Models
{
    public partial class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CapitalId { get; set; }
        public double Area { get; set; }
        public int Population { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual City Сapital { get; set; }
    }
}
