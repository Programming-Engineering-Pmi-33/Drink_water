using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

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
        public Registration()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.email = textBoxEmail.Text;
            this.password = textBoxPassword.Text;

            if (IsValid())
            {
                if (IsUnique(this.username, this.email, this.password))
            {
                
                    if (password == textBoxConfirmPassword.Text)
                    {
                        int salt = CreateRandomSalt();

                        string hashedPassword = ComputeSaltedHash(this.password, salt);
                        Users user = new Users(username, email, hashedPassword,salt.ToString());
                        db.Users.Add(user);
                        db.SaveChanges();
                        Login login = new Login();
                        login.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Passwords don`t match");

                    }
                }
            }
        }


        public int CreateRandomSalt()
        {
            Byte[] _saltBytes = new Byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_saltBytes);

            return ((((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) +
              (((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]));
        }

        public string ComputeSaltedHash(string password,int salt)
        {
            // Create Byte array of password string
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(password);

            // Create a new salt
            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(salt >> 24);
            _saltBytes[1] = (byte)(salt >> 16);
            _saltBytes[2] = (byte)(salt >> 8);
            _saltBytes[3] = (byte)(salt);

            // append the two arrays
            Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }

        //private string EncodePassword(string password)
        //{
        //    byte[] encodedDataBytes = new byte[password.Length];
        //    encodedDataBytes = System.Text.Encoding.UTF8.GetBytes(password);
        //    string encodedPassword = Convert.ToBase64String(encodedDataBytes);
        //    return encodedPassword;
        //}
        private bool IsUnique(string username, string email, string password)
        {
            bool isUnique = true;
            var resultName = (from data in db.Users
                              where data.Username == username
                              select data.Username).ToList();
            if (resultName.Count() > 0)
            {
                labelUsername.Visibility = Visibility.Visible;
                labelUsername.Foreground = Brushes.Red;
                labelUsername.Content = "Username is reserved";
                isUnique = false;
                //MessageBox.Show("Username is reserved", "Validation Error");
                //return false;
            }
            else
            {
                labelUsername.Content = "";
                labelUsername.Visibility = Visibility.Hidden;
            }
            var resultEmail = (from data in db.Users
                               where data.Email == email
                               select data.Email).ToList();
            if (resultEmail.Count() > 0)
            {
                labelEmail.Visibility = Visibility.Visible;
                labelEmail.Foreground = Brushes.Red;
                labelEmail.Content = "Email is reserved";
                isUnique = false;
                //MessageBox.Show("Email is reserved", "Validation Error");
                //return false;
            }
            else
            {
                labelEmail.Content = "";
                labelEmail.Visibility = Visibility.Hidden;
            }
            var resultPassword = (from data in db.Users
                                  where data.Password == password
                                  select data.Password).ToList();
            if (resultPassword.Count() > 0)
            {
                labelPassword.Visibility = Visibility.Visible;
                labelPassword.Foreground = Brushes.Red;
                labelPassword.Content = "Password is reserved";
                isUnique = false;
                //MessageBox.Show("Password is reserved", "Validation Error");
                //return false;
            }
            else
            {
                labelPassword.Content = "";
                labelPassword.Visibility = Visibility.Hidden;
            }
            return isUnique;
        }
        private bool IsValid()
        {
            bool isCorrect = true;

            if (string.IsNullOrWhiteSpace((string)this.username) == true)
            {
                labelUsername.Visibility = Visibility.Visible;
                labelUsername.Foreground = Brushes.Red;
                labelUsername.Content = "Username is required";
                isCorrect = false;
                
            }
            else
            {
                labelUsername.Content = "";
                labelUsername.Visibility = Visibility.Hidden;
            }
            if (this.username.Count(char.IsLetter) < 2)
            {
                labelUsername.Visibility = Visibility.Visible;
                labelUsername.Foreground = Brushes.Red;
                labelUsername.Content = "Username must contain at least 2 letters";
                isCorrect = false;
                
            }
            else
            {
                labelUsername.Content = "";
                labelUsername.Visibility = Visibility.Hidden;
            }
            if (string.IsNullOrWhiteSpace((string)this.email) == true)
            {

                labelEmail.Visibility = Visibility.Visible;
                labelEmail.Foreground = Brushes.Red;
                labelEmail.Content = "Email is required";
                isCorrect = false;
                
            }
            else
            {
               labelEmail.Content = "";
                labelEmail.Visibility = Visibility.Hidden;
            }
            // (?:[a - z0 - 9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])
            if (!this.email.ToString().Contains("@gmail.com"))
            {
                labelEmail.Visibility = Visibility.Visible;
                labelEmail.Foreground = Brushes.Red;
                labelEmail.Content = "Wrong e-mail address";
                isCorrect = false;
                
            }
            else
            {
                labelEmail.Content = "";
                labelEmail.Visibility = Visibility.Hidden;
            }
            if (string.IsNullOrWhiteSpace(this.password))
            {

                labelPassword.Visibility = Visibility.Visible;
                labelPassword.Foreground = Brushes.Red;
                labelPassword.Content = "Password is required";
                isCorrect = false;
                
            }
            else
            {
                labelPassword.Content = "";
                labelPassword.Visibility = Visibility.Hidden;
            }
            if (this.password.ToString().Length < 8)
            {

                labelPassword.Visibility = Visibility.Visible;
                labelPassword.Foreground = Brushes.Red;
                labelPassword.Content = "Password is too short";
                isCorrect = false;
               
            }
            else
            {
                labelPassword.Content = "";
                labelPassword.Visibility = Visibility.Hidden;
            }
            if (this.password.ToString().Count(char.IsLetter) < 2)
            {

                labelPassword.Visibility = Visibility.Visible;
                labelPassword.Foreground = Brushes.Red;
                labelPassword.Content = "Password must contain at least 2 letters";
                isCorrect = false;
                
            }
            else
            {
                labelPassword.Content = "";
                labelPassword.Visibility = Visibility.Hidden;
            }
            if (this.password.ToString().Count(char.IsUpper) < 2)
            {
                labelPassword.Visibility = Visibility.Visible;
                labelPassword.Foreground = Brushes.Red;
                labelPassword.Content = "Password must contain at least 2 or more uppercase letter";
                isCorrect = false;
                
            }
            else
            {
                labelPassword.Content = "";               
                labelPassword.Visibility = Visibility.Hidden;
            }
            return isCorrect;
        }
    }
}
