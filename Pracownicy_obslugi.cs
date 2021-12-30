using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Sekretariat_szko³y_WPF
{
    public partial class MainWindow : Window
    {
        private List<Pracownik_obslugi> ShowStaffMembers()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\Pracownicy_obslugi.txt");

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var pola = line.Split("\t").ToList();

                        var staffMember = new Pracownik_obslugi()
                        {
                            Imie = pola[0],
                            Imie_drugie = pola[1],
                            Nazwisko = pola[2],
                            Nazwisko_rodowe = pola[3],
                            Imie_matki = pola[4],
                            Imie_ojca = pola[5],
                            Data_urodzenia = pola[6],
                            Pesel = pola[7],
                            Plec = pola[8],
                            Etat = pola[9],
                            Opis_stanowiska = pola[10],
                            Data_zatrudnienia = pola[11],
                            Zdjecie_absolute = null,
                            Zdjecie_relative = null
                        };

                        staffMember.Zdjecie_relative = (pola.Count == 13)
                            ? pola[12]
                            : null;

                        staffMember.Zdjecie_absolute = (staffMember.Zdjecie_relative != null)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + staffMember.Zdjecie_relative)
                            : NoImage;

                        staffMembers.Add(staffMember);
                    }
                }
            }
            return staffMembers;
        }

        private void PracownicyObslugi_ClearSortButtonClick(object sender, RoutedEventArgs e)
        {
            ClearSortPracownicyObslugi();
        }

        private void ClearSortPracownicyObslugi()
        {
            PracownicyObslugi_sortColNum.SelectedIndex = 0;
            PracownicyObslugi_sortAscDesc.SelectedIndex = 0;
            ClearSortDataGrid(DG_Dane_PracownicyObslugi);
        }

        private void PracownicyObslugi_SortButtonClick(object sender, RoutedEventArgs e)
        {
            PracownicyObslugiSort();
        }

        private void PracownicyObslugiSort()
        {
            if (PracownicyObslugi_sortColNum.SelectedItem != null && PracownicyObslugi_sortAscDesc.SelectedItem != null)
                SortDataGrid(DG_Dane_PracownicyObslugi,
                    PracownicyObslugi_sortColNum.SelectedIndex + 1,
                    (PracownicyObslugi_sortAscDesc.SelectedIndex == 0)
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending
                );
        }
    }
}