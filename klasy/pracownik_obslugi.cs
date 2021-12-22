using System;

namespace Sekretariat_szko³y_WPF
{
    public class Pracownik_obslugi : Osoba
    {
        public string Etat { get; set; }
        public string Opis_stanowiska { get; set; }
        public string Data_zatrudnienia { get; set; }
    }
}