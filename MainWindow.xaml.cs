using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sekretariat_szkoły_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Uczen> students;
        List<Nauczyciel> teachers;
        List<Pracownik_obslugi> staffMembers;

        public MainWindow()
        {
            InitializeComponent();

            ReportUpdate();

            Uczniowie_SortButton.Click += Uczniowie_SortButtonClick;
            Nauczyciele_SortButton.Click += Nauczyciele_SortButtonClick;
            PracownicyObslugi_SortButton.Click += PracownicyObslugi_SortButtonClick;

            Uczniowie_ClearSortButton.Click += Uczniowie_ClearSortButtonClick;
            Nauczyciele_ClearSortButton.Click += Nauczyciele_ClearSortButtonClick;
            PracownicyObslugi_ClearSortButton.Click += PracownicyObslugi_ClearSortButtonClick;

            Uczniowie_searchColNum.SelectionChanged += Uczniowie_ComboBoxChange;
            Uczniowie_SearchButton.Click += SearchUczniowie;
            Uczniowie_ClearSearchButton.Click += ClearSearchUczniowie;

            //Uczniowie_SaveButton.Click += dataUpdate;
        }

        private void Uczniowie_ComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            var CB = sender as ComboBox;

            Uczniowie_SearchText.IsEnabled = CB.SelectedIndex != 6;
            Uczniowie_SearchForDate.IsEnabled = !(CB.SelectedIndex != 6);

            Uczniowie_SearchText.Text = "";
            Uczniowie_SearchForDate.SelectedIndex = 0;
        }

        private void SearchUczniowie(object sender, RoutedEventArgs e)
        {
            ClearSortUczniowie();
            students = new List<Uczen>();
            DG_Dane_Uczniowie.ItemsSource = SearchStudents();
            DG_Dane_Uczniowie.Items.Refresh();
        }

        private void ClearSearchUczniowie(object sender, RoutedEventArgs e)
        {
            ClearSortUczniowie();
            Uczniowie_searchColNum.SelectedIndex = 0;
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
            if (Uczniowie_sortColNum.SelectedItem != null && Uczniowie_sortAscDesc.SelectedItem != null)
                SortDataGrid(DG_Dane_Uczniowie,
                    Uczniowie_sortColNum.SelectedIndex + 1,
                    (Uczniowie_sortAscDesc.SelectedIndex == 0)
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending
                );
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

        private void Uczniowie_ClearSortButtonClick(object sender, RoutedEventArgs e)
        {
            ClearSortUczniowie();
        }

        private void ClearSortUczniowie()
        {
            Uczniowie_sortColNum.SelectedIndex = 0;
            Uczniowie_sortAscDesc.SelectedIndex = 0;
            ClearSortDataGrid(DG_Dane_Uczniowie);
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

        private void ReportUpdate()
        {
            students = new List<Uczen>();
            teachers = new List<Nauczyciel>();
            staffMembers = new List<Pracownik_obslugi>();

            DG_Dane_Uczniowie.ItemsSource = ShowStudents();
            DG_Dane_Nauczyciele.ItemsSource = ShowTeachers();
            DG_Dane_PracownicyObslugi.ItemsSource = ShowStaffMembers();

            DG_Dane_Uczniowie.Items.Refresh();
            DG_Dane_Nauczyciele.Items.Refresh();
            DG_Dane_PracownicyObslugi.Items.Refresh();
        }

        /*private void dataUpdate(object sender, RoutedEventArgs e)
        {
            switch((sender as Button).Name)
            {
                case "Uczniowie_SaveButton":
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\Uczniowie.txt");
                        bool append = false;

                        foreach (Uczen u in students)
                        {
                            string data = u.ToString();

                            SaveIntoDatabase("Uczniowie", data, append);

                            append = true;
                        }
                    }
                break;
            }
        }*/

        private void LoadFromFileToDB(object sender, RoutedEventArgs e)
        {
            OpenFileDialog LoadFile = OpenTxtFileManually();

            if (LoadFile.ShowDialog() == true)
            {
                IEnumerable<string> lines = File.ReadLines(LoadFile.FileName, Encoding.UTF8);

                foreach (string line in lines)
                {
                    string Type = line.Substring(0, 1);
                    string RestOfData = line.Substring(2);

                    Dictionary<string, string> types = new Dictionary<string, string>()
                    {
                        {"U", "Uczniowie"},
                        {"N", "Nauczyciele" },
                        {"P", "Pracownicy_obslugi" }
                    };

                    SaveIntoDatabase(types.GetValueOrDefault(Type), RestOfData, true);
                }
                ReportUpdate();

                return;
            }
            
            MessageBox.Show("Nie udało się załadować pliku!");
        }

        private List<Uczen> ShowStudents()
        {
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
                            : noImage();

                        students.Add(student);
                    }
                }
            }
            return students;
        }

        private List<Uczen> SearchStudents()
        {
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
                            : noImage();

                        bool toShow = false;

                        if (Uczniowie_searchColNum.SelectedIndex != 6 && Uczniowie_SearchText.Text != null)
                        {
                            switch(Uczniowie_searchColNum.SelectedIndex)
                            {
                                case 0:
                                    {
                                        if (!student.Imie.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 1:
                                    {
                                        if (!student.Imie_drugie.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 2:
                                    {
                                        if (!student.Nazwisko.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (!student.Nazwisko_rodowe.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 4:
                                    {
                                        if (!student.Imie_matki.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 5:
                                    {
                                        if (!student.Imie_ojca.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 7:
                                    {
                                        if (!student.Pesel.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 8:
                                    {
                                        if (!student.Plec.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 9:
                                    {
                                        if (!student.Klasa.Equals(Uczniowie_SearchText.Text))
                                            toShow = true;
                                        break;
                                    }
                                case 10:
                                    {
                                        if(!student.Grupy.Equals(Uczniowie_SearchText.Text))
                                        {
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
                                        }

                                        break;
                                    }
                            }
                        }

                        if(!toShow)
                            students.Add(student);
                    }
                }
            }
            return students;
        }

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
                            : noImage();

                        teachers.Add(teacher);
                    }
                }
            }
            return teachers;
        }

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
                            : noImage();

                        staffMembers.Add(staffMember);
                    }
                }
            }
            return staffMembers;
        }

        private static OpenFileDialog OpenTxtFileManually()
        {
            return new OpenFileDialog()
            {
                DefaultExt = "txt",
                Filter = "Text Files (.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
        }

        private void SaveIntoDatabase(string type, string data, bool append)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\");

            if (!Directory.Exists(path))
            {
                CreateDirectory(path);
            }

            path += type + ".txt";

            if (!File.Exists(path))
            {
                CreateFile(path);
            }

            if (append)
                File.AppendAllText(path, data + Environment.NewLine);
            else
                File.WriteAllText(path, data + Environment.NewLine);
        }

        /*private void SaveImageIntoDatabase(string img_url)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\");

            if (!Directory.Exists(path))
            {
                CreateDirectory(path);
            }

            path += img_url + ".png";

            if (!File.Exists(path))
            {
                CreateFile(path);
            }
        }*/

        private static void CreateFile(string path)
        {
            File.Create(path).Dispose();
        }

        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        private string noImage()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\NO_IMAGE.png");
        }

        private void CopyImage(string sourcePath, string destinationPath) => File.Copy(sourcePath, destinationPath);

        private static void SortDataGrid(DataGrid DG, int colIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var col = DG.Columns[colIndex];

            DG.Items.SortDescriptions.Clear();

            DG.Items.SortDescriptions.Add(new SortDescription(col.SortMemberPath, sortDirection));

            foreach (var column in DG.Columns)
            {
                column.SortDirection = null;
            }

            col.SortDirection = sortDirection;

            DG.Items.Refresh();
        }

        private static void ClearSortDataGrid(DataGrid DG)
        {
            foreach (var column in DG.Columns)
            {
                column.SortDirection = null;
            }

            DG.Items.SortDescriptions.Clear();
            DG.Items.Refresh();
        }
    }
}