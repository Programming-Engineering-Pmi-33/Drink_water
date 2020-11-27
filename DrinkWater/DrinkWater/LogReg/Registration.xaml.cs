namespace DrinkWater.LogReg
{
    using System.Windows;
    using DrinkWater.Services;

    /// <summary>
    /// Interaction logic for Registration.xaml.
    /// </summary>
    public partial class Registration : Window
    {
        private string username;
        private string email;
        private string password;
        private UsersService _usersService;
        private ValidationService _validationService;

        public Registration()
        {
            InitializeComponent();
            _usersService = UsersService.GetService;
            _validationService = new ValidationService(_usersService);
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            // зробити дизайн під ваерфрейми.
            this.username = textBoxUsername.Text;
            this.email = textBoxEmail.Text;
            this.password = textBoxPassword.Text;

            if (_validationService.IsValid(labelUsername, username, labelEmail, email, labelPassword, password))
            {
                if (password == textBoxConfirmPassword.Text)
                {
                    labelPasswordConfirm.Visibility = Visibility.Hidden;
                    int salt = EncryptionService.CreateRandomSalt();

                    string hashedPassword = EncryptionService.ComputeSaltedHash(this.password, salt);
                    User user = new User(username, email, hashedPassword, salt);

                    _usersService.RegisterUser(user);

                    Login login = new Login();
                    login.Show();
                    this.Close();
                }
                else
                {
                    ValidationService.SetError(labelPasswordConfirm, "Passwords don`t match");
                }
            }
        }
    }
}
