namespace DrinkWater.LogReg
{
    using System.Linq;
    using System.Windows;
    using DrinkWater.Services;

    /// <summary>
    /// Interaction logic for Login.xaml.
    /// </summary>
    public partial class Login : Window
    {
        private string username;
        private string password;
        private UsersService _usersService;

        public Login()
        {
            InitializeComponent();
            _usersService = UsersService.GetService;
        }

        private void buttonCreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        public bool IsInDatabase(string item)
        {
            if (item == null)
            {
                return false;
            }

            return true;
        }

        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.password = textBoxPassword.Text;

            var salt = _usersService.GetUserSalt(username);

            if (salt != null)
            {
                labelUsername.Visibility = Visibility.Hidden;

                var userId = _usersService.GetUserId(username, password, salt);

                if (userId > 0)
                {
                    labelPassword.Visibility = Visibility.Hidden;
                    Logger.InitLogger();

                    Logger.Log.Info($"User {username} logged into system.");
                    SessionUser sessionUser = new SessionUser((long)userId, username);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.GetSessionUser(sessionUser);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ValidationService.SetError(labelPassword, "Incorrect password");
                }
            }
            else
            {
                ValidationService.SetError(labelUsername, "No such username in database");
            }
        }
    }
}
