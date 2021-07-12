using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EdytorTekstowy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

    public static class CustomCommands
    {
        public static readonly RoutedUICommand About = new RoutedUICommand
            (
                "O nas",
                "About",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F2)
                }
            );
        public static readonly RoutedUICommand QuitApp = new RoutedUICommand
            (
                "Wyjście",
                "QuitApp",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Q, ModifierKeys.Control)
                }
            );
    }

    public partial class MainWindow : Window
    {
        string FileDir;
        private bool FileCreated = false;
        private bool TextWasChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Bez tytułu";
            TextContent.Document.Blocks.Clear();
            FileCreated = false;
            TextWasChanged = false;
            TextContent.FontSize = 12;
            FontSizeNum.Text = "12";
            FontSizeNum.MaxLength = 3;
        }

/*
 * NOWY
 */

        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(TextWasChanged == true)
            {
                SaveChanges saveChanges = new SaveChanges();
                saveChanges.ShowDialog();
                if (saveChanges.SaveAllChanges == "y")
                {
                    Save_Executed(sender, e);
                    TextContent.Document.Blocks.Clear();
                    TextWasChanged = false;
                    FileCreated = false;
                    this.Title = "Bez tytułu";
                }
                else if (saveChanges.SaveAllChanges == "n")
                {
                    TextContent.Document.Blocks.Clear();
                    TextWasChanged = false;
                    this.Title = "Bez tytułu";
                    FileCreated = false;
                }
                else;
            }
            else
            {
                FileCreated = false;
                TextContent.Document.Blocks.Clear();
                TextWasChanged = false;
                this.Title = "Bez tytułu";
            }          
        }

/*
 * OTWÓRZ
 */

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Plik tekstowy (*.rtf)|*.rtf";
            OpenFile.FilterIndex = 0;
            OpenFile.RestoreDirectory = true;
            if (OpenFile.ShowDialog() == true)
            {
                using (FileStream OpenFileStream = new FileStream(OpenFile.FileName, FileMode.Open))
                {
                    TextRange FileContent = new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd);
                    FileContent.Load(OpenFileStream, DataFormats.Rtf);
                    this.Title = System.IO.Path.GetFileName(OpenFile.FileName) + " - EdytorTekstowy";
                    TextWasChanged = false;
                    FileCreated = true;
                    FileDir = OpenFile.FileName;
                }
            }
        }

/*
 * ZAPISZ
 */

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (TextWasChanged == false && new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd).Text.Length - 2 <= 0)
                e.CanExecute = false;
            else if (TextWasChanged == false)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (FileCreated == true)
            {
                using (FileStream SaveFileStream = new FileStream(FileDir, FileMode.Truncate)) { }
                using (FileStream SaveFileStream = new FileStream(FileDir, FileMode.Append))
                {
                    TextRange FileContent = new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd);
                    FileContent.Save(SaveFileStream, DataFormats.Rtf);
                    TextWasChanged = false;
                }
            }
            else
            {
                SaveFileDialog SaveFile = new SaveFileDialog();
                SaveFile.Filter = "Plik tekstowy (*.rtf)|*.rtf";
                SaveFile.FilterIndex = 0;
                SaveFile.RestoreDirectory = true;
                if (SaveFile.ShowDialog() == true)
                {
                    using (FileStream SaveFileStream = new FileStream(SaveFile.FileName, FileMode.Create))
                    {
                        TextRange FileContent = new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd);
                        FileContent.Save(SaveFileStream, DataFormats.Rtf);
                        this.Title = System.IO.Path.GetFileName(SaveFile.FileName) + " - EdytorTekstowy";
                        FileCreated = true;
                        FileDir = SaveFile.FileName;
                        TextWasChanged = false;
                    }
                }
            }
        }

/*
 * ZAPISZ JAKO 
 */
        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd).Text.Length -2 <= 0)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog SaveAsFile = new SaveFileDialog();
            SaveAsFile.Filter = "Plik tekstowy (*.rtf)|*.rtf";
            SaveAsFile.FilterIndex = 0;
            SaveAsFile.RestoreDirectory = true;
            if (SaveAsFile.ShowDialog() == true)
            {
                using (FileStream SaveFileStream = new FileStream(SaveAsFile.FileName, FileMode.Create))
                {
                    TextRange FileContent = new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd);
                    FileContent.Save(SaveFileStream, DataFormats.Rtf);
                    this.Title = System.IO.Path.GetFileName(SaveAsFile.FileName) + " - EdytorTekstowy";
                    TextWasChanged = false;
                    FileDir = SaveAsFile.FileName;
                    FileCreated = true;
                }
            }
        }

/*
 * WYJŚCIE
 */

        private void QuitApp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void QuitApp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TextWasChanged == true)
            {
                SaveChanges saveChanges = new SaveChanges();
                saveChanges.ShowDialog();
                if (saveChanges.SaveAllChanges == "y")
                {
                    Save_Executed(sender, e);
                    Application.Current.Shutdown();
                }
                else if (saveChanges.SaveAllChanges == "n")
                {
                    Application.Current.Shutdown();
                }
                else;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

/*
 * POMOC
 */

        private void Help_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("To tyle z pomocy", "Pomoc", MessageBoxButton.OK);
        }

/*
 * O PROGRAMIE
 */

        private void About_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Autor: Krzysztof Piotrowski", "Edytor Tekstowy", MessageBoxButton.OK);
        }



        private void TextContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextWasChanged = true;
        }

        private void FontSizeNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextContent.FontSize = float.Parse(FontSizeNum.Text);
                FontSizeNum.Text = float.Parse(FontSizeNum.Text).ToString();
            }
            catch(Exception)
            {
                FontSizeNum.Text = "12";
                TextContent.FontSize = 12;
            }
        }
    }
}
