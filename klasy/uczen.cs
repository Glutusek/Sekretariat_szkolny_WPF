using System.Collections.Generic;

namespace Sekretariat_szko�y_WPF
{
    public class Uczen : Osoba
    {
        public string Klasa { get; set; }
        public List<string> Grupy { get; set; }
    }
}