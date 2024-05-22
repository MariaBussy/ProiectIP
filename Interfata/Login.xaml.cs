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

namespace Interfata
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool IsAuthenticated = false;
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsAuthenticated = true;
            this.Close(); // inchide fereastra dupa autentificare
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void Button_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Project developed by Busaga Maria, Ciosnar Dragos,Asofronie Rares and Adam Iasmina", "Microtask", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

