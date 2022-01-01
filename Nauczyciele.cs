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
        private List<Nauczyciel> GetTeachers()
        {
            teachers.Clear();

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

        private List<Nauczyciel> SearchTeachers()
        {
            bool error = false;

            teachers.Clear();

            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\Nauczyciele.txt");

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null && !error)
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

                        teacher.Zdjecie_relative = (pola.Count == 12)
                            ? pola[11]
                            : null;

                        teacher.Zdjecie_absolute = (teacher.Zdjecie_relative != null)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + teacher.Zdjecie_relative)
                            : NoImage;

                        bool toShow = false;

                        if (Nauczyciele_SearchColNum.SelectedIndex != 6 && Nauczyciele_SearchColNum.SelectedIndex != 11 && Nauczyciele_SearchColNum.SelectedIndex != 12 && Nauczyciele_SearchText.Text != null)
                        {
                            switch (Nauczyciele_SearchColNum.SelectedIndex)
                            {
                                case 0:
                                    {
                                        if (teacher.Imie.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 1:
                                    {
                                        if (teacher.Imie_drugie.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 2:
                                    {
                                        if (teacher.Nazwisko.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (teacher.Nazwisko_rodowe.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 4:
                                    {
                                        if (teacher.Imie_matki.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 5:
                                    {
                                        if (teacher.Imie_ojca.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 7:
                                    {
                                        if (teacher.Pesel.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 8:
                                    {
                                        if (teacher.Plec.Equals(Nauczyciele_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 9:
                                    {
                                        try
                                        {
                                            if (teacher.Wychowawstwo.Equals(Convert.ToBoolean(Nauczyciele_SearchText.Text)))
                                                toShow = true;
                                        }
                                        catch (FormatException)
                                        {
                                            MessageBox.Show("Wychowawstwo przyjmuje tylko wartoœci True lub False!");
                                            error = true;
                                        }
                                        break;
                                    }
                                case 10:
                                    {
                                        if (teacher.Przedmioty.Equals(Nauczyciele_SearchText.Text))
                                        {
                                            toShow = true;
                                            break;
                                        }

                                        string[] subjects = teacher.Przedmioty.Split(", ");
                                        bool anySubjectGood = false;

                                        foreach (string subject in subjects)
                                        {
                                            if (subject.Equals(Nauczyciele_SearchText.Text))
                                            {
                                                anySubjectGood = true;
                                                break;
                                            }
                                        }

                                        if (anySubjectGood)
                                            toShow = true;

                                        break;
                                    }
                            }
                        }
                        else if (Nauczyciele_SearchColNum.SelectedIndex == 11 && Nauczyciele_SearchHoursNum != null)
                        {
                            try
                            {
                                int searchHours = int.Parse(Nauczyciele_SearchHoursNum.Text);

                                int sumOfHours = 0;

                                string[] classes = teacher.Ile_naucza.Split(", ");

                                foreach (string c in classes)
                                {
                                    sumOfHours += int.Parse(c.Split(" ")[1]);
                                }

                                switch (Nauczyciele_SearchHours.SelectedIndex)
                                {
                                    case 0:
                                        toShow = sumOfHours < searchHours;
                                        break;

                                    case 1:
                                        toShow = sumOfHours <= searchHours;
                                        break;

                                    case 2:
                                        toShow = searchHours < sumOfHours;
                                        break;

                                    case 3:
                                        toShow = searchHours <= sumOfHours;
                                        break;

                                    case 4:
                                        toShow = sumOfHours == searchHours;
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Niepoprawna liczba!");
                                error = true;
                            }
                        }
                        else if ((Nauczyciele_SearchColNum.SelectedIndex == 6 || Nauczyciele_SearchColNum.SelectedIndex == 12) && Nauczyciele_SelectedDate.SelectedDate != null)
                        {
                            DateTime teacherDate;

                            if (Nauczyciele_SearchColNum.SelectedIndex == 6)
                                teacherDate = DateTime.Parse(teacher.Data_urodzenia);
                            else
                                teacherDate = DateTime.Parse(teacher.Data_zatrudnienia);

                            DateTime selectedDate = (DateTime)Nauczyciele_SelectedDate.SelectedDate;

                            switch (Nauczyciele_SearchForDate.SelectedIndex)
                            {
                                case 0:
                                    toShow = teacherDate < selectedDate;
                                    break;

                                case 1:
                                    toShow = teacherDate <= selectedDate;
                                    break;

                                case 2:
                                    toShow = selectedDate < teacherDate;
                                    break;

                                case 3:
                                    toShow = selectedDate <= teacherDate;
                                    break;

                                case 4:
                                    toShow = teacherDate == selectedDate;
                                    break;
                            }
                        }

                        if (toShow)
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
            Nauczyciele_SortColNum.SelectedIndex = 0;
            Nauczyciele_SortAscDesc.SelectedIndex = 0;
            ClearSortDataGrid(DG_Dane_Nauczyciele);
        }

        private void Nauczyciele_ComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            var CB = sender as ComboBox;

            Nauczyciele_SearchText.IsEnabled = CB.SelectedIndex != 6 && CB.SelectedIndex != 11 && CB.SelectedIndex != 12;
            Nauczyciele_SearchHours.IsEnabled = CB.SelectedIndex == 11;
            Nauczyciele_SearchHoursNum.IsEnabled = CB.SelectedIndex == 11;
            Nauczyciele_SearchForDate.IsEnabled = CB.SelectedIndex == 6 || CB.SelectedIndex == 12;
            Nauczyciele_SelectedDate.IsEnabled = CB.SelectedIndex == 6 || CB.SelectedIndex == 12;

            Nauczyciele_SearchText.Text = "";
            Nauczyciele_SearchHours.SelectedIndex = 0;
            Nauczyciele_SearchHoursNum.Text = "";
            Nauczyciele_SearchForDate.SelectedIndex = 0;
            Nauczyciele_SelectedDate.SelectedDate = default;
        }

        private void SearchNauczyciele(object sender, RoutedEventArgs e)
        {
            ClearSortNauczyciele();
            DG_Dane_Nauczyciele.ItemsSource = SearchTeachers();
            DG_Dane_Nauczyciele.Items.Refresh();
        }

        private void ClearSearchNauczyciele(object sender, RoutedEventArgs e)
        {
            ClearSortNauczyciele();
            Nauczyciele_SearchColNum.SelectedIndex = 0;
            Nauczyciele_SearchText.IsEnabled = true;
            Nauczyciele_SearchText.Text = "";
            Nauczyciele_SearchForDate.IsEnabled = false;
            Nauczyciele_SearchForDate.SelectedIndex = 0;
            ReportUpdate();
        }

        private void Nauczyciele_SortButtonClick(object sender, RoutedEventArgs e)
        {
            SortNauczyciele();
        }

        private void SortNauczyciele()
        {
            if (Nauczyciele_SortColNum.SelectedItem != null && Nauczyciele_SortAscDesc.SelectedItem != null)
                SortDataGrid(DG_Dane_Nauczyciele,
                    Nauczyciele_SortColNum.SelectedIndex + 1,
                    (Nauczyciele_SortAscDesc.SelectedIndex == 0)
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending
                );
        }
    }
}