using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog LoadFile = new OpenFileDialog()
            {
                DefaultExt = "txt",
                Filter = "Text Files (.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if(LoadFile.ShowDialog() == true)
            {
                IEnumerable<string> lines = File.ReadLines(LoadFile.FileName, Encoding.UTF8);

                foreach(string line in lines)
                {
                    string Type = line.Substring(0,1);
                    string RestOfData = line.Substring(2);

                    Dictionary<string, string> types = new Dictionary<string, string>()
                    {
                        {"U", "Uczniowie"},
                        {"N", "Nauczyciele" },
                        {"P", "Pracownicy_obslugi" }
                    };

                    SaveIntoDatabase(types.GetValueOrDefault(Type), RestOfData);
                }
            }
        }

        private void SaveIntoDatabase(string type, string data)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"baza_danych\");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += type + ".txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            File.AppendAllText(path, data + Environment.NewLine);
        }
    }
}