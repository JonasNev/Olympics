using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Models
{
    public class FilterModel
    {
        public string FilerCountry { get; set; } // 0 || lithuania || estija yra saliu dropdownas
        public string FilerSport { get; set; } // 0 || basketball || swimming yra sporto dropdownas
        public string FilerActivity { get; set; } // -1 || 0 || 1 teamactivity dropdownas "pasirinkti actiitivy tipa"(-1), "not team"(0), "team" (1)
    }
}
