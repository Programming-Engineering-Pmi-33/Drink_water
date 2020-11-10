using DrinkWater.Services;
using System.Windows;

namespace DrinkWater.LogReg
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        private string username;
        private string email;
        private string password;
        private ValidationService _validationService;

        public Registration()
        {
            InitializeComponent();
            _validationService = new ValidationService(db);
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            //зробити дизайн під ваерфрейми.
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
                    Users user = new Users(username, email, hashedPassword, salt.ToString());
                    db.Users.Add(user);
                    db.SaveChanges();
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
