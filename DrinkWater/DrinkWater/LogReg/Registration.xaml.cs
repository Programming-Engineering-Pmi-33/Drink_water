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
        private UsersService usersService;
        private ValidationService validationService;

        public Registration()
        {
            InitializeComponent();
            usersService = UsersService.GetService;
            validationService = new ValidationService(usersService);
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.email = textBoxEmail.Text;
            this.password = textBoxPassword.Text;

            if (validationService.IsValid(labelUsername, username, labelEmail, email, labelPassword, password))
            {
                if (password == textBoxConfirmPassword.Text)
                {
                    labelPasswordConfirm.Visibility = Visibility.Hidden;
                    long salt = EncryptionService.CreateRandomSalt();

                    string hashedPassword = EncryptionService.ComputeSaltedHash(this.password, salt);
                    User user = new User(username, email, hashedPassword, salt);

                    usersService.RegisterUser(user);

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
