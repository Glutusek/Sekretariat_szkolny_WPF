using System;
using System.Collections.Generic;

namespace Sekretariat_szko�y_WPF
{
    public class Nauczyciel : Osoba
    {
        public bool Wychowawstwo { get; set; }
        public string Przedmioty { get; set; }
        public string Ile_naucza { get; set; }
        public string Data_zatrudnienia { get; set; }
    }
}