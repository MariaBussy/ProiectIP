using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Interfata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Login lWin;
        private string _contentTime; //pentru pop up ul de add time, se salveaza texul inainte de taskul adaugat
        public MainWindow()
        {
            InitializeComponent();

            Loaded +=MainWindow_Loaded;
        }

        //afisare pagina de login + facut MainForm ul invizibil iar cand se face autentificarea se inchide
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lWin = new Login();
            lWin.Owner = this;
            lWin.Show();
            this.Visibility = Visibility.Collapsed;
            lWin.Closed += LoginWindow_Closed;
        }

        //in momentul in care se autentifica utilizatorul, se face main form ul vizibil si se inchide fereastra de login
        private void LoginWindow_Closed(object sender, EventArgs e)
        {
           
            if (lWin.IsAuthenticated) 
            {
                this.Visibility = Visibility.Visible; 
            }
            else
            {
                this.Close(); 
            }
        }

        //functie cand apas back sa se intoarca la pagina de login initiala
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow_Loaded(sender, e);
        }

        //functii pentru tratare pop-up la add time
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _contentTime = timeLabel.Content.ToString();
            timeButton.IsEnabled = false;
            timeLabel.Content += "5 :";//aici trebuie sa fie adaugat in functie de task ul ales
            timePopup.IsOpen = true;
        }
        //TREBUIE TRATATA EXCEPTIA DACA SE INTRODUCE TEXT IN LOC DE NUMAR(sau altele precu numere negative etc)!!!
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            timeLabel.Content = _contentTime;
            timeButton.IsEnabled = true;
            timePopup.IsOpen = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            timeLabel.Content = _contentTime;
            timeButton.IsEnabled = true;
            timePopup.IsOpen = false;
        }
    }
}


