using System;
using System.Collections.Generic;

namespace Sekretariat_szko³y_WPF
{
    public struct Ilosc_godzin
    {
        public string Klasa;
        public int Godziny;

        public Ilosc_godzin(string  Klasa, int Godziny)
        {
            this.Klasa = Klasa;
            this.Godziny = Godziny;
        }
    }

    public class Nauczyciel : Osoba
    {
        public string Wychowawstwo { get; set; }
        public List<string> Przedmioty { get; set; }
        public List<Ilosc_godzin> Ile_naucza { get; set; }
        public DateTime Data_zatrudnienia { get; set; }

        public Nauczyciel()
        {
            Ile_naucza = new List<Ilosc_godzin>();
        }
    }
}