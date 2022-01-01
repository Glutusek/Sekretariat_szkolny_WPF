using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sekretariat_szkoły_WPF
{
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
            Uczniowie_ClearSortButton.Click += Uczniowie_ClearSortButtonClick;
            Uczniowie_SearchColNum.SelectionChanged += Uczniowie_ComboBoxChange;
            Uczniowie_SearchButton.Click += SearchUczniowie;
            Uczniowie_ClearSearchButton.Click += ClearSearchUczniowie;

            Nauczyciele_SortButton.Click += Nauczyciele_SortButtonClick;
            Nauczyciele_ClearSortButton.Click += Nauczyciele_ClearSortButtonClick;
            Nauczyciele_SearchColNum.SelectionChanged += Nauczyciele_ComboBoxChange;
            Nauczyciele_SearchButton.Click += SearchNauczyciele;
            Nauczyciele_ClearSearchButton.Click += ClearSearchNauczyciele;

            PracownicyObslugi_SortButton.Click += PracownicyObslugi_SortButtonClick;
            PracownicyObslugi_ClearSortButton.Click += PracownicyObslugi_ClearSortButtonClick;
            PracownicyObslugi_SearchColNum.SelectionChanged += PracownicyObslugi_ComboBoxChange;
            PracownicyObslugi_SearchButton.Click += SearchPracownicyObslugi;
            PracownicyObslugi_ClearSearchButton.Click += ClearSearchPracownicyObslugi;
        }

        private void ReportUpdate()
        {
            students = new List<Uczen>();
            teachers = new List<Nauczyciel>();
            staffMembers = new List<Pracownik_obslugi>();

            DG_Dane_Uczniowie.ItemsSource = GetStudents();
            DG_Dane_Nauczyciele.ItemsSource = GetTeachers();
            DG_Dane_PracownicyObslugi.ItemsSource = GetStaffMembers();

            DG_Dane_Uczniowie.Items.Refresh();
            DG_Dane_Nauczyciele.Items.Refresh();
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

        private static void CreateFile(string path) => File.Create(path).Dispose();

        private static void CreateDirectory(string path) => Directory.CreateDirectory(path);

        private string NoImage => Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\NO_IMAGE.png");

        private void CopyFile(string sourcePath, string destinationPath) => File.Copy(sourcePath, destinationPath);

        private static void CopyDirectory(string sourcePath, string destinationPath)
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);

            if(!dir.Exists)
            {
                MessageBox.Show("Nie można znaleźć ścieżki!");
            }
            else
            {
                DirectoryInfo[] dirs = dir.GetDirectories();

                Directory.CreateDirectory(destinationPath);

                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    string path = Path.Combine(destinationPath, file.Name);
                    file.CopyTo(path, false);
                }

                foreach(DirectoryInfo subdir in dirs)
                {
                    string path = Path.Combine(destinationPath, subdir.Name);
                    CopyDirectory(subdir.FullName, path);
                }
            }
        }

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

        private string GetSelectedTabName() => ((TabItem)Sekretariat.SelectedItem).Header.ToString();

        private void GenerateWindowReportButton_Click(object sender, RoutedEventArgs e) => GenerateReport(GetSelectedTabName());

        private void GenerateAllDBReportButton_Click(object sender, RoutedEventArgs e)
        {
            string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\");

            SaveFileDialog SFD = new SaveFileDialog()
            {
                InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
                FileName = DateTime.Now.ToString("dd.M.yyyy HH.mm.ss") + " - backup bazy danych"
            };

            MessageBox.Show("Przy backupie bazy danych skup się tylko na miejscu docelowym zapisu oraz nazwaniu bazy. Zostanie utworzony specjalny folder z podaną nazwą!");

            if(SFD.ShowDialog() == true)
            {
                string chosenName = SFD.FileName.Substring(SFD.FileName.LastIndexOf("\\"));
                string destinationPath = Path.GetDirectoryName(SFD.FileName) + chosenName;

                CopyDirectory(sourcePath, destinationPath);
            }
        }

        private void GenerateReport(string type)
        {
            MessageBox.Show("Pamiętaj, że raport będzie zawierać układ zgodny z wyświetlonym, tzn. może zawierać sortowanie lub wyszukiwanie dokonane przez Ciebie!");

            SaveFileDialog SFD = new SaveFileDialog()
            {
                InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
                AddExtension = true,
                DefaultExt = "txt"
            };

            switch (type)
            {
                case "Uczniowie":
                    {
                        SFD.FileName = DateTime.Now.ToString("dd.M.yyyy HH.mm.ss") + " - raport uczniów";

                        if (SFD.ShowDialog() == true)
                        {
                            string textToSave = "";

                            foreach (Uczen u in DG_Dane_Uczniowie.ItemsSource)
                            {
                                textToSave += u.ToString() + Environment.NewLine;
                            }

                            File.WriteAllText(SFD.FileName, textToSave);
                        }
                        break;
                    }

                case "Nauczyciele":
                    {
                        SFD.FileName = DateTime.Now.ToString("dd.M.yyyy HH.mm.ss") + " - raport nauczycieli";

                        if (SFD.ShowDialog() == true)
                        {
                            string textToSave = "";

                            foreach (Nauczyciel n in DG_Dane_Nauczyciele.ItemsSource)
                            {
                                textToSave += n.ToString() + Environment.NewLine;
                            }

                            File.WriteAllText(SFD.FileName, textToSave);
                        }
                        break;
                    }

                case "Pracownicy obsługi":
                    {
                        SFD.FileName = DateTime.Now.ToString("dd.M.yyyy HH.mm.ss") + " - raport pracowników obsługi";

                        if (SFD.ShowDialog() == true)
                        {
                            string textToSave = "";

                            foreach (Pracownik_obslugi p in DG_Dane_PracownicyObslugi.ItemsSource)
                            {
                                textToSave += p.ToString() + Environment.NewLine;
                            }

                            File.WriteAllText(SFD.FileName, textToSave);
                        }
                        break;
                    }

                default:
                    MessageBox.Show("Nie możesz dokonać raportu tego okna!");
                    break;
            }
        }
    }
}