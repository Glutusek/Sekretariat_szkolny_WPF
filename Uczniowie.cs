using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sekretariat_szko³y_WPF
{
    public partial class MainWindow : Window
    {
        private List<Uczen> GetStudents()
        {
            students.Clear();

            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\Uczniowie.txt");

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var pola = line.Split("\t").ToList();

                        var student = new Uczen()
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
                            Klasa = pola[9],
                            Grupy = pola[10],
                            Zdjecie_absolute = null,
                            Zdjecie_relative = null
                        };

                        student.Zdjecie_relative = (pola.Count == 12)
                            ? pola[11]
                            : null;

                        student.Zdjecie_absolute = (student.Zdjecie_relative != null)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + student.Zdjecie_relative)
                            : NoImage;

                        students.Add(student);
                    }
                }
            }
            return students;
        }

        private List<Uczen> SearchStudents()
        {
            students.Clear();

            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\Uczniowie.txt");

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var pola = line.Split("\t").ToList();

                        var student = new Uczen()
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
                            Klasa = pola[9],
                            Grupy = pola[10],
                            Zdjecie_absolute = null,
                            Zdjecie_relative = null
                        };

                        student.Zdjecie_relative = (pola.Count == 12)
                            ? pola[11]
                            : null;

                        student.Zdjecie_absolute = (student.Zdjecie_relative != null)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + student.Zdjecie_relative)
                            : NoImage;

                        bool toShow = false;

                        if (Uczniowie_SearchColNum.SelectedIndex != 6 && Uczniowie_SearchText.Text != null)
                        {
                            switch (Uczniowie_SearchColNum.SelectedIndex)
                            {
                                case 0:
                                    {
                                        if (student.Imie.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 1:
                                    {
                                        if (student.Imie_drugie.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 2:
                                    {
                                        if (student.Nazwisko.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (student.Nazwisko_rodowe.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 4:
                                    {
                                        if (student.Imie_matki.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 5:
                                    {
                                        if (student.Imie_ojca.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 7:
                                    {
                                        if (student.Pesel.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 8:
                                    {
                                        if (student.Plec.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 9:
                                    {
                                        if (student.Klasa.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 10:
                                    {
                                        if (student.Grupy.Equals(Uczniowie_SearchText.Text))
                                        {
                                            toShow = true;
                                            break;
                                        }

                                        string[] groups = student.Grupy.Split(", ");
                                        bool anyGroupGood = false;

                                        foreach (string group in groups)
                                        {
                                            if (group.Equals(Uczniowie_SearchText.Text))
                                            {
                                                anyGroupGood = true;
                                                break;
                                            }
                                        }

                                        if (anyGroupGood)
                                            toShow = true;

                                        break;
                                    }
                            }
                        }
                        else if (Uczniowie_SearchColNum.SelectedIndex == 6 && Uczniowie_SelectedDate.SelectedDate != null)
                        {
                            DateTime studentDate = DateTime.Parse(student.Data_urodzenia);
                            DateTime selectedDate = (DateTime)Uczniowie_SelectedDate.SelectedDate;

                            switch (Uczniowie_SearchForDate.SelectedIndex)
                            {
                                case 0:
                                    toShow = studentDate < selectedDate;
                                    break;

                                case 1:
                                    toShow = studentDate <= selectedDate;
                                    break;

                                case 2:
                                    toShow = selectedDate < studentDate;
                                    break;

                                case 3:
                                    toShow = selectedDate <= studentDate;
                                    break;

                                case 4:
                                    toShow = studentDate == selectedDate;
                                    break;
                            }
                        }

                        if (toShow)
                            students.Add(student);
                    }
                }
            }
            return students;
        }

        private void Uczniowie_ClearSortButtonClick(object sender, RoutedEventArgs e)
        {
            ClearSortUczniowie();
        }

        private void ClearSortUczniowie()
        {
            Uczniowie_SortColNum.SelectedIndex = 0;
            Uczniowie_SortAscDesc.SelectedIndex = 0;
            ClearSortDataGrid(DG_Dane_Uczniowie);
        }

        private void Uczniowie_ComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            var CB = sender as ComboBox;

            Uczniowie_SearchText.IsEnabled = CB.SelectedIndex != 6;
            Uczniowie_SearchForDate.IsEnabled = CB.SelectedIndex == 6;
            Uczniowie_SelectedDate.IsEnabled = CB.SelectedIndex == 6;

            Uczniowie_SearchText.Text = "";
            Uczniowie_SearchForDate.SelectedIndex = 0;
            Uczniowie_SelectedDate.SelectedDate = default;
        }

        private void SearchUczniowie(object sender, RoutedEventArgs e)
        {
            ClearSortUczniowie();
            DG_Dane_Uczniowie.ItemsSource = SearchStudents();
            DG_Dane_Uczniowie.Items.Refresh();
        }

        private void ClearSearchUczniowie(object sender, RoutedEventArgs e)
        {
            ClearSortUczniowie();
            Uczniowie_SearchColNum.SelectedIndex = 0;
            Uczniowie_SearchText.IsEnabled = true;
            Uczniowie_SearchText.Text = "";
            Uczniowie_SearchForDate.IsEnabled = false;
            Uczniowie_SearchForDate.SelectedIndex = 0;
            ReportUpdate();
        }

        private void Uczniowie_SortButtonClick(object sender, RoutedEventArgs e)
        {
            SortUczniowie();
        }

        private void SortUczniowie()
        {
            if (Uczniowie_SortColNum.SelectedItem != null && Uczniowie_SortAscDesc.SelectedItem != null)
                SortDataGrid(DG_Dane_Uczniowie,
                    Uczniowie_SortColNum.SelectedIndex + 1,
                    (Uczniowie_SortAscDesc.SelectedIndex == 0)
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending
                );
        }
    }
}