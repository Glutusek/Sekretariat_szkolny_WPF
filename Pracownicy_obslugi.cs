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
        private List<Pracownik_obslugi> GetStaffMembers()
        {
            staffMembers.Clear();

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

        private List<Pracownik_obslugi> SearchStaffMembers()
        {
            staffMembers.Clear();

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

                        bool toShow = false;

                        if (PracownicyObslugi_SearchColNum.SelectedIndex != 6 && PracownicyObslugi_SearchColNum.SelectedIndex != 11 && PracownicyObslugi_SearchText.Text != null)
                        {
                            switch (PracownicyObslugi_SearchColNum.SelectedIndex)
                            {
                                case 0:
                                    {
                                        if (staffMember.Imie.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 1:
                                    {
                                        if (staffMember.Imie_drugie.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 2:
                                    {
                                        if (staffMember.Nazwisko.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (staffMember.Nazwisko_rodowe.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 4:
                                    {
                                        if (staffMember.Imie_matki.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 5:
                                    {
                                        if (staffMember.Imie_ojca.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 7:
                                    {
                                        if (staffMember.Pesel.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 8:
                                    {
                                        if (staffMember.Plec.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 9:
                                    {
                                        if (staffMember.Etat.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 10:
                                    {
                                        if (staffMember.Opis_stanowiska.Equals(PracownicyObslugi_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                            }
                        }
                        else if ((PracownicyObslugi_SearchColNum.SelectedIndex == 6 || PracownicyObslugi_SearchColNum.SelectedIndex == 11) && PracownicyObslugi_SelectedDate.SelectedDate != null)
                        {
                            DateTime staffMemberDate;

                            if(PracownicyObslugi_SearchColNum.SelectedIndex == 6)
                                staffMemberDate = DateTime.Parse(staffMember.Data_urodzenia);
                            else
                                staffMemberDate = DateTime.Parse(staffMember.Data_zatrudnienia);

                            DateTime selectedDate = (DateTime)PracownicyObslugi_SelectedDate.SelectedDate;

                            switch (PracownicyObslugi_SearchForDate.SelectedIndex)
                            {
                                case 0:
                                    toShow = staffMemberDate < selectedDate;
                                    break;

                                case 1:
                                    toShow = staffMemberDate <= selectedDate;
                                    break;

                                case 2:
                                    toShow = selectedDate < staffMemberDate;
                                    break;

                                case 3:
                                    toShow = selectedDate <= staffMemberDate;
                                    break;

                                case 4:
                                    toShow = staffMemberDate == selectedDate;
                                    break;
                            }
                        }

                        if (toShow)
                            staffMembers.Add(staffMember);
                    }
                }
            }
            return staffMembers;
        }

        private void PracownicyObslugi_ComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            var CB = sender as ComboBox;

            PracownicyObslugi_SearchText.IsEnabled = CB.SelectedIndex != 6 && CB.SelectedIndex != 11;
            PracownicyObslugi_SearchForDate.IsEnabled = !(CB.SelectedIndex != 6 && CB.SelectedIndex != 11);
            PracownicyObslugi_SelectedDate.IsEnabled = !(CB.SelectedIndex != 6 && CB.SelectedIndex != 11);

            PracownicyObslugi_SearchText.Text = "";
            PracownicyObslugi_SearchForDate.SelectedIndex = 0;
            PracownicyObslugi_SelectedDate.SelectedDate = default;
        }

        private void SearchPracownicyObslugi(object sender, RoutedEventArgs e)
        {
            ClearSortPracownicyObslugi();
            DG_Dane_PracownicyObslugi.ItemsSource = SearchStaffMembers();
            DG_Dane_PracownicyObslugi.Items.Refresh();
        }

        private void ClearSearchPracownicyObslugi(object sender, RoutedEventArgs e)
        {
            ClearSortPracownicyObslugi();
            PracownicyObslugi_SearchColNum.SelectedIndex = 0;
            PracownicyObslugi_SearchText.IsEnabled = true;
            PracownicyObslugi_SearchText.Text = "";
            PracownicyObslugi_SearchForDate.IsEnabled = false;
            PracownicyObslugi_SearchForDate.SelectedIndex = 0;
            ReportUpdate();
        }

        private void PracownicyObslugi_ClearSortButtonClick(object sender, RoutedEventArgs e)
        {
            ClearSortPracownicyObslugi();
        }

        private void ClearSortPracownicyObslugi()
        {
            PracownicyObslugi_SortColNum.SelectedIndex = 0;
            PracownicyObslugi_SortAscDesc.SelectedIndex = 0;
            ClearSortDataGrid(DG_Dane_PracownicyObslugi);
        }

        private void PracownicyObslugi_SortButtonClick(object sender, RoutedEventArgs e)
        {
            PracownicyObslugiSort();
        }

        private void PracownicyObslugiSort()
        {
            if (PracownicyObslugi_SortColNum.SelectedItem != null && PracownicyObslugi_SortAscDesc.SelectedItem != null)
                SortDataGrid(DG_Dane_PracownicyObslugi,
                    PracownicyObslugi_SortColNum.SelectedIndex + 1,
                    (PracownicyObslugi_SortAscDesc.SelectedIndex == 0)
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending
                );
        }
    }
}