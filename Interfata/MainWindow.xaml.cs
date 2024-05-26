/**************************************************************************
 *  Author:      Ciosnar Dragos-Alexandru                                 *
 *  File:        MainWindow.xaml.cs                                       *                             
 *  E-mail:      dragos-alexandru.ciosnar@student.tuiasi.ro               *
 *  Description: Partea de implementare cu interfata grafica a proiectului*
 *  pentru fereastra principala a aplicatiei.                             *
 *                                                                        *
 **************************************************************************/



using DataBaseDLL;
using System;
using System.Data.SQLite;
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
using PROIECT;
using DataBase;

namespace Interfata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    // aici initializam baza de date in primul rand,
    public partial class MainWindow : Window
    {
        Login lWin;
        private string _contentTime; //pentru pop up ul de add time, se salveaza texul inainte de taskul adaugat
        public UserRepository userRepository;
        public TaskRepository taskRepository;
        public UserTaskRepository userTaskRepository;
        private List<IUser> _members;//lista cu toti membrii afisati dupa ce se face log in
        private IUser _currentUser;
        private int _checkedTeam = 0;
        private string _previosUser = "";
        private List<string> _tasksToIncrease;

        /// <summary>
        /// Constructor pentru fereastra principala unde initializam fereastra, ne conectam la baza de date si declaram obiectele pentru functionalitatile tabelelor.
        /// De asemenea, apelam si functia de incarcare a paginii de login.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //initializez tot ce tine de baza de date + obiectele care implementeaza functionalitatile din fiecare tabel
            Database database = new Database();
            userRepository = new UserRepository();
            taskRepository = new TaskRepository();
            userTaskRepository = new UserTaskRepository();
            Loaded += MainWindow_Loaded;
        }

        //afisare pagina de login + facut MainForm ul invizibil iar cand se face autentificarea se inchide
        /// <summary>
        /// Aceasta functie incarca pagina de login, o afiseaza si ascunde pagina principala (main form-ul).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lWin = new Login();
            lWin.Owner = this;
            lWin.Show();
            this.Visibility = Visibility.Collapsed;
            lWin.Closed += LoginWindow_Closed;
        }

        //in momentul in care se autentifica utilizatorul, se face main form ul vizibil si se inchide fereastra de login
        /// <summary>
        /// Aceasta functie este apelata in momentul in care autentificarea utilizatorului a fost facuta cu succes.
        /// Se initializeaza clientii si le sunt atribuite task-urile la nivel local. De asemenea, se seteaza starea butoanelor ( in functie de permisiunile utilizatorului curent)
        /// si se afiseaza taskurile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            // se sterg listele de fiecare data cand se iese sau intra in login
            Team_Members.Items.Clear();
            ToDo.Items.Clear();
            Doing.Items.Clear();
            if (lWin.IsAuthenticated)
            {
                InitialiseClients();//initializam clientii ca obiecte de tip client
                InitialiseTasksForClients();//fac initializarea de liste de task uri la nivel local
                SetActiveUserLogin(lWin.username.Text, lWin.password.Password); // setare active user in functie de user si parola
                SetButtonsPermision();
                SetViewTask();
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Functie care creeaza in listele Doing si To Do obiecte de tip CheckBox pentru gestionarea si selectarea taskurilor care vor fi pe parcurs modificate.
        /// </summary>
        private void SetViewTask()
        {
            int number = 0; // contor numar taskuri
            foreach(var task in _currentUser.Tasks) 
            {
                number++;
                CheckBox newCheckBox = new CheckBox
                {
                    FontFamily = new FontFamily("Cascadia Code"),
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Name = "Task" + number,
                    Content = number.ToString() + ":" + task.NumeTask + ":" + task.DescriereTask
                };

                // Crearea unui StackPanel pentru a conține CheckBox și TextBlock
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal

                };

                // Adăugarea CheckBox în StackPanel
                stackPanel.Children.Add(newCheckBox);

                // Crearea unui nou ListBoxItem și setarea StackPanel ca și content
                ListBoxItem newItem = new ListBoxItem
                {
                    Content = stackPanel,
                    Background = new SolidColorBrush(Colors.Pink)
                };

                // verific aici daca se adauga la toDo sau Doing
                if(task.OreLogate > 0)
                {
                    Doing.Items.Add(newItem);
                }
                else
                {
                    ToDo.Items.Add(newItem);
                }
                
            }
        }

        /// <summary>
        /// In aceasta functie se implementeaza afisarea datelor userului curent pe niste label uri si se populeaza tabela de membrii care sunt sub tutela userului curent.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void SetActiveUserLogin(string username, string password)
        {
            var users = userRepository.GetAllUsers();
            foreach (var user in users)
            {
                if(user.Username == username && user.Password== password)
                {
                    // in functie de username si parola se vor afisa team mambers care sunt sub tutela user ului curent
                    PopulateMembers(user.UserId);
                    active_name.Content = user.Name;
                    status.Content = user.UserStat;
                    break;
                }
            }
        }

        /// <summary>
        /// Implementare asemanatoare cu functia SetActiveUserLogin doar ca nu mai populeaza tabela membrii, ci schimba stric userul curent.
        /// Este folosita cand cineva vrea sa vada taskurile unui utilizator sub indrumarea lui.
        /// </summary>
        /// <param name="name"></param>
        private void SetActiveUserName(string name)
        {
            var users = userRepository.GetAllUsers();
            foreach (var user in users)
            {
                if (user.Name == name)
                {
                    // in functie de username si parola se vor afisa team mambers care sunt sub tutela user ului curent
                    active_name.Content = user.Name;
                    status.Content = user.UserStat;
                    break;
                }
            }
        }

        /// <summary>
        /// Functia propriu-zisa de populare a tabelei de membrii pe principiul adaugarii de checkBox uri. Se gestioneaza si evenimentele de checked, unchecked box unde sunt apelate functii corespunzatoare.
        /// </summary>
        /// <param name="teamLeadId"></param>
        private void PopulateMembers(int teamLeadId)
        {
            var users = userRepository.GetAllUsers();
            foreach (var user in users)
            {
                if (user.TeamLeadId == teamLeadId)
                {
                    CheckBox newCheckBox = new CheckBox
                    {
                        FontFamily = new FontFamily("Cascadia Code"),
                        FontSize = 15,
                        FontWeight = FontWeights.Bold,
                        Name = "Member" + user.UserId,
                        Content = user.Name
                    };
                    // leg evenimentele de checked si unchecked la niste functii
                    newCheckBox.Checked += CheckBox_Checked;
                    newCheckBox.Unchecked += CheckBox_Unchecked;

                    // Crearea unui StackPanel pentru a conține CheckBox și TextBlock
                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal

                    };

                    // Adăugarea CheckBox în StackPanel
                    stackPanel.Children.Add(newCheckBox);

                    // Crearea unui nou ListBoxItem și setarea StackPanel ca și content
                    ListBoxItem newItem = new ListBoxItem
                    {
                        Content = stackPanel,
                        Background = new SolidColorBrush(Colors.Pink)
                    };

                    Team_Members.Items.Add(newItem);
                }
            }
        }

        /// <summary>
        /// In aceasta functie sunt initializati toti clientii la nivel local, specificandu-le numele si statutul. Datele sunt extrase din baza de date si membrii sunt adaugti intr-o lista pentru 
        /// gestionarea lor in alte implementari.
        /// </summary>
        private void InitialiseClients()
        {
            var users = userRepository.GetAllUsers();
            _members = new List<IUser>();
            IUser person = new Client { Name = "ERROR", State = new DeveloperState() };//in cazul in care nu intra in niciun if
            foreach (var user in users)
            {
                if (user.UserStat == UserStats.Developer)
                {
                    person = new Client { Name = user.Name, State = new DeveloperState() };
                }
                else if (user.UserStat == UserStats.TeamLeader)
                {
                    person = new Client { Name = user.Name, State = new TeamLeadState() };
                }
                else if (user.UserStat == UserStats.Manager)
                {
                    person = new Client { Name = user.Name, State = new ManagerState() };
                }
                _members.Add(new LoggingUserDecorator(person));
            }
        }

        /// <summary>
        /// Dupa initializarea clientilor la nivel local, pentru fiecare membru ii trebuie extrasa si lista de taskuri din baza de date (lista salvata tot local).
        /// </summary>
        private void InitialiseTasksForClients()
        {
            var users = userRepository.GetAllUsers();
            foreach (var user in users)
            {
                var tasks = userTaskRepository.GetUserTasks(user.UserId);
                for (int i = 0; i < _members.Count; i++)
                {
                    if (_members[i].Name == user.Name)
                    {
                        foreach (var task in tasks)
                        {
                            _members[i].AddTask(task);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Functie care stabileste accesul la butoane in functie de permisiunile utilizatorului curent.
        /// </summary>
        private void SetButtonsPermision()
        {
            foreach (var member in _members)
            {
                if (member.Name == active_name.Content.ToString())
                {
                    _currentUser = member;//setez aici si current userul
                    if (member.State is DeveloperState)
                    {
                        Add_Task.IsEnabled = false;
                        Delete_Task.IsEnabled = false;
                        Modify_Task.IsEnabled = false;
                        Add_Member.IsEnabled = false;
                        Delete_Member.IsEnabled = false;

                    }
                    else if (member.State is TeamLeadState)
                    {
                        Add_Task.IsEnabled = true;
                        Delete_Task.IsEnabled = true;
                        Modify_Task.IsEnabled = true;
                        Add_Member.IsEnabled = false;
                        Delete_Member.IsEnabled = false;
                    }
                    else if (member.State is ManagerState)
                    {
                        Add_Task.IsEnabled = true;
                        Delete_Task.IsEnabled = true;
                        Modify_Task.IsEnabled = true;
                        Add_Member.IsEnabled = true;
                        Delete_Member.IsEnabled = true;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Aici este implementata gestiunea userilor selectati din lista de membrii. Se salveaza userul selectat anterior pentru a se putea intoarce la el cand se deselecteaza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (_checkedTeam < 1)
            {
                _checkedTeam++;
                // implementez switch ul activ user ului
                CheckBox check = (CheckBox)sender;
                TextBlock textBlock = FindVisualChild<TextBlock>(check);
                _previosUser = _currentUser.Name;
                // se sterge lista cu membrii curenti(mai putin cel selectat) si se reincarca
                SetActiveUserName(textBlock.Text);
                SetButtonsPermision();
                // schimb afisarea task urilor in functie de userul activ
                ToDo.Items.Clear();
                Doing.Items.Clear();
                SetViewTask();
            }
            else
            {
                CheckBox check = (CheckBox)sender;
                // pentru a deconecta evenimentul de unchecked(cand atribuiam false se autoapela unchecked si nu doream asta)
                check.Checked -= CheckBox_Checked;
                check.Unchecked -= CheckBox_Unchecked;

                check.IsChecked = false;

                check.Checked += CheckBox_Checked;
                check.Unchecked += CheckBox_Unchecked;


            }

        }

        /// <summary>
        /// Aici se gestioneaza situatia deselectarii unui user si revenirea la starea initiala.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_checkedTeam > 0)
            {
                _checkedTeam--;
                //si setez user ul curent ca fiind cel anterior selectiei
                SetActiveUserName(_previosUser);
                SetButtonsPermision();
                // schimb afisarea task urilor in functie de userul activ
                ToDo.Items.Clear();
                Doing.Items.Clear();
                SetViewTask();
            }
        }

        /// <summary>
        /// Functie simpla de implementare a butonului de back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow_Loaded(sender, e);
        }

        //functii pentru tratare pop-up la add time
        /// <summary>
        /// Gestionarea apasarii butonului de add time. Se verifica taskurile selectate si se introduc intr-o lista pentru a le adauga un timp de lucru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _tasksToIncrease = new List<string>();// lista in care se vor gasi numele taskurilor a caror timp trebuie crescut
            _contentTime = timeLabel.Content.ToString();
            timeButton.IsEnabled = false;
          
            // accesez fieare checkBox din lista ToDo
            foreach(var item in ToDo.Items)
            {
                var container = ToDo.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (container != null) 
                {
                    var checkBox = FindVisualChild<CheckBox>(container);
                    if (checkBox.IsChecked == true)
                    {
                        // aflu numele care este inainte de :
                        string[] index = checkBox.Content.ToString().Split(':');
                        string name = index[1];
                        timeLabel.Content += " " + checkBox.Name.ToString();
                        _tasksToIncrease.Add(name);
                    }
                }
            }

            // la fel si la doing
            foreach (var item in Doing.Items)
            {
                var container = Doing.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (container != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(container);
                    if (checkBox.IsChecked == true)
                    {
                        // aflu numele care este inainte de :
                        string[] index = checkBox.Content.ToString().Split(':');
                        string name = index[1];
                        timeLabel.Content += " " + checkBox.Name.ToString();
                        _tasksToIncrease.Add(name);
                    }
                }
            }
  
            timePopup.IsOpen = true;
        }

        /// <summary>
        /// Verifica daca textul introdus in timeTextBox este valid si actualizeaza taskurile corespunzator.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int result;
                Int32.TryParse(timeTextBox.Text, out result);
                if (timeTextBox.Text == "" || _tasksToIncrease.Count == 0 || result == 0)
                {
                    throw new Exception();
                }
                // trebuie sa dau increase atat la nivel de taskuri local cat si cele din baza de date
                foreach (var task in _tasksToIncrease)
                {
                    TaskLibrary.Task t = taskRepository.GetTaskByName(task);
                    t.OreLogate += Double.Parse(timeTextBox.Text);
                    taskRepository.UpdateTask(t);
                }

                foreach (var membru in _members)
                {
                    foreach (var task in _tasksToIncrease)
                    {
                        foreach (var tasc in membru.Tasks)
                            if (tasc.NumeTask == task)
                            {

                                TaskLibrary.Task t = taskRepository.GetTaskByName(task);
                                t.OreLogate += Double.Parse(timeTextBox.Text);
                                membru.ModifyTask(tasc.NumeTask, t);
                            }
                    }
                }
                // actualizez listele
                ToDo.Items.Clear();
                Doing.Items.Clear();
                InitialiseClients();
                InitialiseTasksForClients();
                SetButtonsPermision();
                SetViewTask();
                timeButton.IsEnabled = true;
                timePopup.IsOpen = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Time was not added correctly or you didn't select a task!","Error Add Time!");
            }
        }

        /// <summary>
        /// Inchide popup-ul pentru adaugare timp si reactiveaza butonul.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            timeButton.IsEnabled = true;
            timePopup.IsOpen = false;
        }

        /// <summary>
        /// Deschide popup-ul pentru adaugarea unui task nou si dezactiveaza butonul de adaugare.
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add_Task.IsEnabled = false;
            taskPOPUP.IsOpen = true;
        }

        /// <summary>
        /// Adauga un task nou pentru utilizatorul curent si il salveaza in baza de date.
        /// </summary>
        private void OkButtonTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var users = userRepository.GetAllUsers();
                Add_Task.IsEnabled = true;
                taskPOPUP.IsOpen = false;
                //adaugarea task ului
                TaskLibrary.Task task = new TaskLibrary.Task(taskName.Text, _currentUser.Name);
                task.DescriereTask = taskDesc.Text;
                foreach (var member in _members)
                {
                    if (member == _currentUser)
                    {
                        // pentru nivel local
                        member.AddTask(task);
                        // pentru nivel de baze de date
                        foreach (var user in users)
                        {
                            if (member.Name == user.Name)
                            {
                                taskRepository.CreateTask(task);
                                var tasks = taskRepository.GetAllTasks();
                                foreach (var ta in tasks)
                                {
                                    if (ta.NumeTask == task.NumeTask)
                                    {
                                        userTaskRepository.AddUserTask(ta.NumeTask, user.UserId);
                                    }

                                }

                            }
                        }
                    }
                }
                ToDo.Items.Clear();
                Doing.Items.Clear();
                InitialiseClients();
                InitialiseTasksForClients();
                SetViewTask();
            }catch (Exception)
            {
                MessageBox.Show("Task was introduced incorrectly!", "Error Add Task!");
            }
        }

        /// <summary>
        /// Inchide popup-ul pentru adaugarea unui task nou si reactiveaza butonul de adaugare.
        /// </summary>
        private void CancelButtonTask_Click(object sender, RoutedEventArgs e)
        {
            Add_Task.IsEnabled = true;
            taskPOPUP.IsOpen = false;
        }

        /// <summary>
        /// Deschide popup-ul pentru adaugarea unui membru nou si dezactiveaza butonul de adaugare.
        /// </summary>
        private void Add_Member_Click(object sender, RoutedEventArgs e)
        {
            Add_Member.IsEnabled = false;
            Member_Add.IsOpen= true;
        }

        /// <summary>
        /// Adauga un membru nou si il salveaza in baza de date.
        /// </summary>
        private void OkAddMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MemberAddName.Text == "" || MemberAddUsername.Text == "" || MemberAddPass.Text == "" || MemberAddName.Text.Length < 5)
                {
                    throw new Exception();
                }
                string userStatStr = "2";
                int teamLeadId = 0;
                if (_currentUser.State is ManagerState)
                {
                    userStatStr = "1";
                }
                
                UserStats userStat;
                if (!Enum.TryParse(userStatStr, out userStat))
                {
                    throw new Exception();
                }
                //caut id ul current memberului actual
                var users = userRepository.GetAllUsers();
                foreach (var user in users)
                {
                    // tratez cazurile cand adaug la current member un nou membru sau adaug la altcineva un nou membru
                    if (AddMemberUser.Text == "")
                    {
                        if (user.Name == _currentUser.Name)
                        {
                            teamLeadId = user.UserId;
                        }
                    }
                    else
                    {
                        if (user.Name == AddMemberUser.Text)
                        {
                            teamLeadId = user.UserId;
                            /*
                            if(userStat-1 == 0)
                            {
                                throw new Exception();// daca se incearca introducerea unui user in subtutela unui developer
                            }
                            */

                            userStatStr = "2";
                            if (!Enum.TryParse(userStatStr, out userStat))
                            {
                                throw new Exception();
                            }
                        }
                    }

                }
                User newUser = new User
                {
                    Name = MemberAddName.Text,
                    UserStat = userStat,
                    TeamLeadId = teamLeadId,
                    Tasks = new List<TaskLibrary.Task>(),
                    Username = MemberAddUsername.Text,
                    Password = MemberAddPass.Text
                };

                userRepository.CreateUser(newUser); //metoda care defapt insereaza user ul in tabela!!!!
                Add_Member.IsEnabled = true;
                Member_Add.IsOpen = false;

                // reinitializa membrii si tabelele
                Team_Members.Items.Clear();
                
                InitialiseClients();
                InitialiseTasksForClients();
                SetActiveUserLogin(lWin.username.Text, lWin.password.Password);

            }
            catch (Exception)
            {
                MessageBox.Show("Data inseted incorrectly!", "Error Add Member!");
            }
        }

        /// <summary>
        /// Inchide popup-ul pentru adaugarea unui membru nou si reactiveaza butonul de adaugare.
        /// </summary>
        private void CancelAddMember_Click(object sender, RoutedEventArgs e) 
        {
         
            Add_Member.IsEnabled = true;
            Member_Add.IsOpen = false;
        }

        /// <summary>
        /// Deschide popup-ul pentru adaugarea unui membru nou si dezactiveaza butonul de adaugare.
        /// </summary>
        private void Delete_Member_Click(object sender, RoutedEventArgs e)
        {
            Delete_Member.IsEnabled = false;
            Member_Delete.IsOpen = true;
        }

        // parte de delete, atentie la situatia cu delete manager sau teamlead(stergere in cascada)
        // si stergerea userului activ
        /// <summary>
        /// Sterge un membru si actualizeaza baza de date, avand grija de stergerea in cascada.
        /// </summary>
        private void OkDeleteMember_Click(object sender, RoutedEventArgs e)
        {
            var users = userRepository.GetAllUsers();
            foreach(var user in users)
            {

                if(user.Name == MemberDeleteName.Text)
                {
                    // stergem si cei de sub teamlead
                    if(user.UserStat == UserStats.TeamLeader)
                    {
                        foreach (var uti in users)
                        {
                            if(user.UserId == uti.TeamLeadId)
                            {
                                userRepository.DeleteUser(uti.UserId);
                            }
                        }
                    }

                    // cand stergem team lead-ul
                    if (user.UserStat == UserStats.Manager)
                    {
                        foreach (var uti in users)
                        {
                            if (user.UserId == uti.TeamLeadId)
                            {
                                foreach(var u in users)
                                {
                                    if (uti.UserId == u.TeamLeadId)
                                    {
                                        userRepository.DeleteUser(u.UserId);
                                    }
                                        
                                }
                                userRepository.DeleteUser(uti.UserId);
                            }
                        }
                    }

                    if(user.Name == _currentUser.Name)
                    {
                        MainWindow_Loaded(sender, e);
                    }

                    userRepository.DeleteUser(user.UserId);
                }
            }
            Delete_Member.IsEnabled = true;
            Member_Delete.IsOpen = false;

            // reinitializa membrii si tabelele
            Team_Members.Items.Clear();
            InitialiseClients();
            InitialiseTasksForClients();
            SetActiveUserLogin(lWin.username.Text, lWin.password.Password); // setare active user in functie de user si parola
            SetButtonsPermision();
        }

        /// <summary>
        /// Inchide popup-ul pentru stergerea unui membru si reactiveaza butonul de stergere.
        /// </summary>
        private void CancelDeleteMember_Click(object sender, RoutedEventArgs e)
        {
            Delete_Member.IsEnabled = true;
            Member_Delete.IsOpen = false;
        }

        /// <summary>
        /// Functie care implementeaza ideea de stergere a unui task selectat din listele To Do si Doing. Cand se sterge se actualizeaza atat la nivel local listele de task uri cat si taskurile din baza de date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Task_Click(object sender, RoutedEventArgs e)
        {

            // accesez fieare checkBox din lista ToDo
            foreach (var item in ToDo.Items)
            {
                var container = ToDo.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (container != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(container);
                    if (checkBox.IsChecked == true)
                    {
                        // aflu numele care este inainte de :
                        string[] index = checkBox.Content.ToString().Split(':');
                        string name_task = index[1];
                        // stergerea din lista de taskuri locale
                        foreach(var membru in _members)
                        {
                            foreach(var task in membru.Tasks)
                            {
                                if(task.NumeTask == name_task)
                                {
                                    membru.RemoveTask(name_task);
                                    break;
                                }
                            }
                        }
                        // stergerea din baz de date
                        var tasks = taskRepository.GetAllTasks();
                        foreach (var task in tasks)
                        {
                            if(task.NumeTask == name_task)
                            {
                                taskRepository.DeleteTask(name_task);
                            }
                        }
                    }
                }
            }

            // la fel si la doing
            foreach (var item in Doing.Items)
            {
                var container = Doing.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (container != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(container);
                    if (checkBox.IsChecked == true)
                    {
                        // aflu numele care este inainte de :
                        string[] index = checkBox.Content.ToString().Split(':');
                        string name_task = index[1];
                        // stergerea din lista de taskuri locale
                        foreach (var membru in _members)
                        {
                            foreach (var task in membru.Tasks)
                            {
                                if (task.NumeTask == name_task)
                                {
                                    membru.RemoveTask(name_task);
                                    break;
                                }
                            }
                        }
                        // stergerea din baz de date
                        var tasks = taskRepository.GetAllTasks();
                        foreach (var task in tasks)
                        {
                            if (task.NumeTask == name_task)
                            {
                                taskRepository.DeleteTask(name_task);
                            }
                        }
                    }
                }
            }

            // actualizez listele
            ToDo.Items.Clear();
            Doing.Items.Clear();
            Team_Members.Items.Clear();
            InitialiseClients();
            InitialiseTasksForClients();
            SetActiveUserLogin(lWin.username.Text, lWin.password.Password); // setare active user in functie de user si parola
            SetButtonsPermision();
            SetViewTask();
        }

        /// <summary>
        ///  Deschide popup-ul pentru adaugarea unui membru nou si dezactiveaza butonul de adaugare.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Modify_Task_Click(object sender, RoutedEventArgs e)
        {
            Modify_Task.IsEnabled = false;
            Task_Modify.IsOpen = true;
        }

        /// <summary>
        /// Functie care implementeaza conceptul de modificare a unui task. Se creeaza un nou obiect task care va fi folosit pentru inlocuirea celui vechi.
        /// Schimbarea se face atat la nivel local cat si la nivel de baze de date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkModifyTask_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (Modify_description.Text == "" || Modify_description.Text.Length < 5)
                {
                    throw new Exception();
                }
                // accesez fieare checkBox din lista ToDo
                foreach (var item in ToDo.Items)
                {
                    var container = ToDo.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                    if (container != null)
                    {
                        var checkBox = FindVisualChild<CheckBox>(container);
                        if (checkBox.IsChecked == true)
                        {
                            // aflu numele care este inainte de :
                            string[] index = checkBox.Content.ToString().Split(':');
                            string name_task = index[1];
                            // modificarea din lista de taskuri locale
                            foreach (var membru in _members)
                            {
                                foreach (var task in membru.Tasks)
                                {
                                    if (task.NumeTask == name_task)
                                    {
                                        TaskLibrary.Task modified_task = new TaskLibrary.Task(name_task, _currentUser.Name);
                                        modified_task.DescriereTask = Modify_description.Text;
                                        modified_task.OreLogate = Int32.Parse(Modify_hours.Text);
                                        membru.ModifyTask(name_task, modified_task);
                                        break;
                                    }
                                }
                            }
                            // modificarea din baz de date
                            var tasks = taskRepository.GetAllTasks();
                            foreach (var task in tasks)
                            {
                                if (task.NumeTask == name_task)
                                {
                                    TaskLibrary.Task modified_task = new TaskLibrary.Task(name_task, _currentUser.Name);
                                    modified_task.DescriereTask = Modify_description.Text;
                                    modified_task.OreLogate = Int32.Parse(Modify_hours.Text);
                                    taskRepository.UpdateTask(modified_task);
                                }
                            }
                        }
                    }
                }

                // la fel si la doing
                foreach (var item in Doing.Items)
                {
                    var container = Doing.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                    if (container != null)
                    {
                        var checkBox = FindVisualChild<CheckBox>(container);
                        if (checkBox.IsChecked == true)
                        {
                            // aflu numele care este inainte de :
                            string[] index = checkBox.Content.ToString().Split(':');
                            string name_task = index[1];
                            // modificarea din lista de taskuri locale
                            foreach (var membru in _members)
                            {
                                foreach (var task in membru.Tasks)
                                {
                                    if (task.NumeTask == name_task)
                                    {
                                        TaskLibrary.Task modified_task = new TaskLibrary.Task(name_task, _currentUser.Name);
                                        modified_task.DescriereTask = Modify_description.Text;
                                        modified_task.OreLogate = Int32.Parse(Modify_hours.Text);
                                        membru.ModifyTask(name_task, modified_task);
                                        break;
                                    }
                                }
                            }
                            // modificarea din baz de date
                            var tasks = taskRepository.GetAllTasks();
                            foreach (var task in tasks)
                            {
                                if (task.NumeTask == name_task)
                                {
                                    TaskLibrary.Task modified_task = new TaskLibrary.Task(name_task, _currentUser.Name);
                                    modified_task.DescriereTask = Modify_description.Text;
                                    modified_task.OreLogate = Int32.Parse(Modify_hours.Text);
                                    taskRepository.UpdateTask(modified_task);
                                }
                            }
                        }
                    }
                }

                // actualizez listele
                ToDo.Items.Clear();
                Doing.Items.Clear();
                Team_Members.Items.Clear();
                InitialiseClients();
                InitialiseTasksForClients();
                SetActiveUserLogin(lWin.username.Text, lWin.password.Password); // setare active user in functie de user si parola
                SetButtonsPermision();
                SetViewTask();


                Modify_Task.IsEnabled = true;
                Task_Modify.IsOpen = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Modify data introduced incorrectly!", "Eroor Modify Task!");
            }
        }

        /// <summary>
        ///  Inchide popup-ul pentru stergerea unui membru si reactiveaza butonul de stergere.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelModifyTask_Click(object sender, RoutedEventArgs e)
        {
            Modify_Task.IsEnabled = true;
            Task_Modify.IsOpen = false;
        }


        /// <summary>
        /// Metoda generica pentru a gasi un copil vizual de un anumit tip.
        /// </summary>
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T tChild)
                {
                    return tChild;
                }
                else
                {
                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

      
    }

}