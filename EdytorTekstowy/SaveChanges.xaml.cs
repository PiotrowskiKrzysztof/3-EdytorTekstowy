using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace EdytorTekstowy
{
    /// <summary>
    /// Logika interakcji dla klasy SaveChanges.xaml
    /// </summary>
    public partial class SaveChanges : Window
    {
        public string SaveAllChanges = "c";
        public SaveChanges()
        {
            InitializeComponent();
            SaveAllChanges = "c";
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            SaveAllChanges = "y";
            this.Close();
        }

        private void Nie_Click(object sender, RoutedEventArgs e)
        {
            SaveAllChanges = "n";
            this.Close();
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            SaveAllChanges = "c";
            this.Close();
        }
    }
}
