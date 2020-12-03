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

        public Login()
        {
            InitializeComponent();
            usersService = UsersService.GetService;
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
