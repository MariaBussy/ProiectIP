/**************************************************************************
 *  Author:      Ciosnar Dragos-Alexandru                                 *
 *  File:        Login.xaml.cs                                            *                             
 *  E-mail:      dragos-alexandru.ciosnar@student.tuiasi.ro               *
 *  Description: Partea de implementare cu interfata grafica a proiectului*
 *  pentru fereastra de login a aplicatiei.                               *
 *                                                                        *
 **************************************************************************/

using DataBaseDLL;
using System.Data.SQLite;
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
using DataBase;

namespace Interfata
{
    public partial class Login : Window
    {
        public bool IsAuthenticated = false;
        public UserRepository userRepository;
        public TaskRepository taskRepository;
        public UserTaskRepository userTaskRepository;

        /// <summary>
        /// Constructor pentru Login
        /// </summary>
        public Login()
        {
            InitializeComponent();
            userRepository = new UserRepository();
            taskRepository = new TaskRepository();
            userTaskRepository = new UserTaskRepository();
        }

        /// <summary>
        /// Verifica daca exista un utilizator in baza de date cu username-ul si parola date
        /// </summary>
        /// <param name="username">Numele de utilizator</param>
        /// <param name="password">Parola</param>
        /// <returns>True daca utilizatorul exista, altfel False</returns>
        private bool VerificareBD(string username, string password)
        {
            var users = userRepository.GetAllUsers();
            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Eveniment pentru butonul de autentificare
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text != "" && password.Password != "")
            {
                if (VerificareBD(username.Text, password.Password) == true)
                {
                    IsAuthenticated = true;
                    this.Close(); // Inchide fereastra dupa autentificare
                }
                else
                {
                    MessageBox.Show("No matching username and password!", "Unregistered User!");
                }

            }
            else
            {
                MessageBox.Show("Add username and password!", "Empty fields!");
            }
        }

        /// <summary>
        /// Eveniment pentru butonul de inchidere a aplicatiei
        /// </summary>
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// Eveniment pentru butonul "About"
        /// </summary>
        private void Button_About(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "file://C://Users//EU//Desktop//ProiectIP-Interfata//Interfata//Interfata//bin//Debug//Microtask.chm");
        }

        /// <summary>
        /// Deschide popup-ul pentru sign in si dezactiveaza butonul de sign in
        /// </summary>
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Sign_Button.IsEnabled = false;
            SignIn.IsOpen = true;
        }

        /// <summary>
        /// Eveniment pentru butonul de confirmare a sign in-ului
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int result;
                Int32.TryParse(Stat.Text, out result);
                if (Name.Text.Count() < 5 || Int32.Parse(Stat.Text) >=3 || Int32.Parse(Stat.Text) < 0)
                {
                    throw new Exception();
                }

                string userStatStr = Stat.Text;
                UserStats userStat;
                if (!Enum.TryParse(userStatStr, out userStat))
                {
                    throw new Exception();
                }
                User newUser = new User
                {
                    Name = Name.Text,
                    UserStat = userStat,
                    TeamLeadId = 0,
                    Tasks = new List<TaskLibrary.Task>(),
                    Username = signin_username.Text,
                    Password = signin_password.Text
                };

                userRepository.CreateUser(newUser); // Insereaza user-ul in baza de date
                Sign_Button.IsEnabled = true;
                SignIn.IsOpen = false;
            }
            catch (Exception)
            {
                MessageBox.Show("You didn't insert the correct data!", "Error Sign In!");
            }
        }

        /// <summary>
        /// Inchide popup-ul pentru sign in si reactiveaza butonul de sign in
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Sign_Button.IsEnabled = true;
            SignIn.IsOpen = false;
        }
    }
}
