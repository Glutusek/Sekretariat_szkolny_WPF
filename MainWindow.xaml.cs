using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

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

            reportUpdate();
        }

        private void reportUpdate()
        {
            students = new List<Uczen>();
            teachers = new List<Nauczyciel>();
            staffMembers = new List<Pracownik_obslugi>();

            DG_Dane_Nauczyciele.ItemsSource = ShowTeachers();
            DG_Dane_Uczniowie.ItemsSource = ShowStudents();
            DG_Dane_PracownicyObslugi.ItemsSource = ShowStaffMembers();

            DG_Dane_Nauczyciele.Items.Refresh();
            DG_Dane_Uczniowie.Items.Refresh();
            DG_Dane_PracownicyObslugi.Items.Refresh();
        }

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

                    SaveIntoDatabase(types.GetValueOrDefault(Type), RestOfData);
                }
                reportUpdate();

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
                            Zdjecie_url = null
                        };

                        student.Zdjecie_url = (pola.Count == 12)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + pola[11])
                            : noImage();

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
                            Zdjecie_url = null
                        };

                        teacher.Zdjecie_url = (pola.Count() == 14)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + pola[13])
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
                            Zdjecie_url = null
                        };

                        staffMember.Zdjecie_url = (pola.Count == 13)
                            ? Path.Combine(Directory.GetCurrentDirectory(), @"zdjecia\" + pola[13])
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

        private void SaveIntoDatabase(string type, string data)
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

            File.AppendAllText(path, data + Environment.NewLine);
        }

        private void SaveIIntoDatabase(string img_url)
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
        }

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
    }
}