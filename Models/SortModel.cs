using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Models
{
    public class SortModel
    {
        public string Sort { get; set; }
        public List<string> SortSelection { get; set; } = new() { "Name", "Surname", "CountryName", "SportName" }; // name || surname || sport || country dropdown
    }
}
