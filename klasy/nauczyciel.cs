using System;
using System.Collections.Generic;

namespace Sekretariat_szko³y_WPF
{
    public struct Lekcja
    {
        string Klasa;
        string Przedmiot;
        string Dzien_tygodnia;
        string Godziny;
    }

    public class Nauczyciel : Osoba
    {
        public string Wychowawstwo { get; set; }
        public List<string> Przedmioty { get; set; }
        public List<Lekcja> Kiedy_naucza { get; set; }
        public DateTime Data_zatrudnienia { get; set; }
    }
}