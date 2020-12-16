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

        /// <summary>
        /// Initializes a new instance of the <see cref="Registration"/> class.
        /// </summary>
        public Registration()
        {
            InitializeComponent();
            usersService = UsersService.GetService;
            validationService = new ValidationService(usersService);
        }

        /// <summary>
        /// Performs signing up and opens login window.
        /// </summary>
        /// <param name="sender">object instance.</param>
        /// <param name="e">RoutedEventArgs instance.</param>
        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.email = textBoxEmail.Text;
            this.password = textBoxPassword.Password;

            if (validationService.IsValid(labelUsername, username, labelEmail, email, labelPassword, password))
            {
                if (password == textBoxConfirmPassword.Password)
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
