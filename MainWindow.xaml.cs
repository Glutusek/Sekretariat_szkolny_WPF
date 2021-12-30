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

            Uczniowie_SearchColNum.SelectionChanged += Uczniowie_ComboBoxChange;
            Uczniowie_SearchButton.Click += SearchUczniowie;
            Uczniowie_ClearSearchButton.Click += ClearSearchUczniowie;

            //Uczniowie_SaveButton.Click += dataUpdate;
        }

        private void ReportUpdate()
        {
            students = new List<Uczen>();
            teachers = new List<Nauczyciel>();
            staffMembers = new List<Pracownik_obslugi>();

            DG_Dane_Uczniowie.ItemsSource = GetStudents();
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
            
            MessageBox.Show("Przerwano proces wczytywania pliku!");
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

        private static void CreateFile(string path) => File.Create(path).Dispose();

        private static void CreateDirectory(string path) => Directory.CreateDirectory(path);

        private string NoImage => Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\NO_IMAGE.png");

        private void CopyImage(string sourcePath, string destinationPath) => File.Copy(sourcePath, destinationPath);

        private static void SortDataGrid(DataGrid DG, int colIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            DataGridColumn col = DG.Columns[colIndex];

            DG.Items.SortDescriptions.Clear();

            DG.Items.SortDescriptions.Add(new SortDescription(col.SortMemberPath, sortDirection));

            foreach (DataGridColumn column in DG.Columns)
            {
                column.SortDirection = null;
            }

            col.SortDirection = sortDirection;

            DG.Items.Refresh();
        }

        private static void ClearSortDataGrid(DataGrid DG)
        {
            foreach (DataGridColumn column in DG.Columns)
            {
                column.SortDirection = null;
            }

            DG.Items.SortDescriptions.Clear();
            DG.Items.Refresh();
        }
    }
}