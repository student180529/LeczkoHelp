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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiniaProdukcyjna
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] id_pracownicy = {"PT987654", "MP123456"};
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (id.Text.Length == 0)
            {
                MessageBox.Show("Nie wprowadzono ID pracownika!!");
            }
            else if (id.Text.Length!=8)
            {
                MessageBox.Show("Błędny format ID");
            }
            else {
                string logowanie = id.Text;
                for (int i = 0; i < id_pracownicy.Length; i++) {
                    if (logowanie == id_pracownicy[i]) {
                        MessageBox.Show("Pomyślnie zalogowano pracownika " + logowanie, "Udane logowanie");
                        i = id_pracownicy.Length;
                        id.Text = "";
                        Linia linia = new Linia();
                        linia.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}
