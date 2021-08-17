using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Models
{
    public class SortFilterModel
    {
        public string Sort { get; set; }
        public List<string> SortSelection { get; set; } = new() { "Name", "Surname", "CountryName", "SportName" }; // name || surname || sport || country dropdown
        public string FilterCountry { get; set; } // 0 || lithuania || estija yra saliu dropdownas
        public string FilterSport { get; set; } // 0 || basketball || swimming yra sporto dropdownas
        public int FilterTeamActivity { get; set; } // -1 || 0 || 1 teamactivity dropdownas "pasirinkti actiitivy tipa"(-1), "not team"(0), "team" (1)
    }
}
