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
        List<Nauczyciel> teachers;

        public MainWindow()
        {
            InitializeComponent();

            teachers = new List<Nauczyciel>();

            DG_Dane.ItemsSource = ShowTeachers();
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

                return;
            }
            
            MessageBox.Show("Nie udało się załadować pliku!");
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

                        teachers.Add(new Nauczyciel()
                        {
                            Id = pola[0],
                            Imie = pola[1],
                            Imie_drugie = pola[2],
                            Nazwisko = pola[3],
                            Nazwisko_panienskie = pola[4],
                            Imie_matki = pola[5],
                            Imie_ojca = pola[6],
                            Pesel = pola[7],
                            Plec = pola[9],
                            Data_urodzenia = pola[10],
                            Wychowawstwo = Convert.ToBoolean(pola[11]),
                            Przedmioty = pola[12],
                            Ile_naucza = pola[13],
                            Data_zatrudnienia = pola[14]
                        });
                    }
                }
            }
            return teachers;
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

        private static void CreateFile(string path)
        {
            File.Create(path).Dispose();
        }

        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}