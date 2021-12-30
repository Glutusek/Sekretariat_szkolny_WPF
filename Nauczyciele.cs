using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Sekretariat_szko³y_WPF
{
    public partial class MainWindow : Window
    {
        private List<Nauczyciel> ShowTeachers()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\Nauczyciele.txt");

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var pola = line.Split("\t").ToList();

                        var teacher = new Nauczyciel()
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
                            Wychowawstwo = Convert.ToBoolean(pola[9]),
                            Przedmioty = pola[10],
                            Ile_naucza = pola[11],
                            Data_zatrudnienia = pola[12],
                            Zdjecie_absolute = null,
                            Zdjecie_relative = null
                        };

                        teacher.Zdjecie_relative = (pola.Count == 14)
                            ? pola[13]
                            : null;

                        teacher.Zdjecie_absolute = (teacher.Zdjecie_relative != null)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + teacher.Zdjecie_relative)
                            : NoImage;

                        teachers.Add(teacher);
                    }
                }
            }
            return teachers;
        }

        private void Nauczyciele_ClearSortButtonClick(object sender, RoutedEventArgs e)
        {
            ClearSortNauczyciele();
        }

        private void ClearSortNauczyciele()
        {
            Nauczyciele_sortColNum.SelectedIndex = 0;
            Nauczyciele_sortAscDesc.SelectedIndex = 0;
            ClearSortDataGrid(DG_Dane_Nauczyciele);
        }

        private void Nauczyciele_SortButtonClick(object sender, RoutedEventArgs e)
        {
            SortNauczyciele();
        }

        private void SortNauczyciele()
        {
            if (Nauczyciele_sortColNum.SelectedItem != null && Nauczyciele_sortAscDesc.SelectedItem != null)
                SortDataGrid(DG_Dane_Nauczyciele,
                    Nauczyciele_sortColNum.SelectedIndex + 1,
                    (Nauczyciele_sortAscDesc.SelectedIndex == 0)
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending
                );
        }
    }
}