namespace DrinkWater.LogReg
{
    using System.Windows;
    using DrinkWater.Services;

    /// <summary>
    /// Interaction logic for Login.xaml.
    /// </summary>
    public partial class Login : Window
    {
        private string username;
        private string password;
        private UsersService usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            InitializeComponent();
            usersService = UsersService.GetService;
            Logger.InitLogger();
        }

        private void ButtonCreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.password = textBoxPassword.Text;

            var salt = usersService.GetUserSalt(username);

            if (salt != 0)
            {
                labelUsername.Visibility = Visibility.Hidden;

                var userId = usersService.GetUserId(username, password, salt);

                if (userId > 0)
                {
                    labelPassword.Visibility = Visibility.Hidden;
                    ExceptionHandler.UnhadledExceprionHandler();
                    Logger.Log.Info($"User {username} logged into system.");
                    SessionUser sessionUser = new SessionUser((long)userId, username);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.SetSessionUser(sessionUser);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ValidationService.SetError(labelPassword, "Incorrect password");
                    Logger.Log.Info($"Inputed wrong password for user {username}");
                }
            }
            else
            {
                ValidationService.SetError(labelUsername, "No such username in database");
            }
        }
    }
}
