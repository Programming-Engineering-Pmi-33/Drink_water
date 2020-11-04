using System.Windows;
using System.Linq;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;

namespace DrinkWater.LogReg
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        private string username;
        private string password;
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        public bool isInDatabase(string item)
        {
            if (item == null)
            {
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.password = textBoxPassword.Text;


            var result = (from data in db.Users
                          where data.Username!=null && data.Username == username
                          select data.Password).FirstOrDefault();
            if (isInDatabase(result))
            {
                labelUsername.Content = "";
                labelUsername.Visibility = Visibility.Hidden;
               
                var salt = (from data in db.Users
                              where data.Username == username
                              select data.Salt).FirstOrDefault();
                string decodedPassword = "qwerty123456";//ComputeSaltedHash(this.password, int.Parse(salt));
                if (decodedPassword==result)
                {
                    labelPassword.Content = "";
                    labelPassword.Visibility = Visibility.Hidden;
                   
                    var id = (from user in db.Users
                              where user.Password == password
                              select user.UserId).FirstOrDefault();
                    SessionUser sessionUser = new SessionUser((long)id, username);
                    MessageBox.Show("it works.");
                    Settings settings = new Settings();
                    settings.GetSessionUser(sessionUser);
                    settings.Show();
                    this.Close();

                }
                else
                {
                    SetError(labelPassword, "Incorrect password");
                   
                    
                }
            }
            else
            {
                SetError(labelUsername, "No such username in database");
                
               
            }
        }

        private bool SetError(System.Windows.Controls.Label errorLabel, string message)
        {

            errorLabel.Visibility = Visibility.Visible;
            errorLabel.Foreground = Brushes.Red;
            errorLabel.Content = message;

            return false;
        }
        public string ComputeSaltedHash(string password, int salt)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(password);

            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(salt >> 24);
            _saltBytes[1] = (byte)(salt >> 16);
            _saltBytes[2] = (byte)(salt >> 8);
            _saltBytes[3] = (byte)(salt);

            Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }
    }
}
