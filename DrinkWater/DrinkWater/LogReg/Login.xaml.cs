using System.Windows;
using System.Linq;
using DrinkWater.Services;

namespace DrinkWater.LogReg
{
    /// <summary>
    /// Interaction logic for Login.xaml
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

        private void buttonCreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }


        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.password = textBoxPassword.Text;

            var salt = usersService.GetUserSalt(username);

            if (salt != null)
            {
                labelUsername.Visibility = Visibility.Hidden;

                var userId = usersService.GetUserId(username, password, salt);

                if (userId > 0)
                {
                    labelPassword.Visibility = Visibility.Hidden;

                    SessionUser sessionUser = new SessionUser((long)userId, username);
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
