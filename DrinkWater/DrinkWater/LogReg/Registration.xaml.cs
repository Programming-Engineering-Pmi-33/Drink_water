using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
                
            
                
                    if (password == textBoxConfirmPassword.Text)
                    {
                        
                        labelPasswordConfirm.Visibility = Visibility.Hidden;
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
                        SetError(labelPasswordConfirm, "Passwords don`t match");
                       
                        

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

        private bool IsValid()
        {

            bool isCorrectUsername = isValidUsername();
            bool isCorrectEmail = isValidEmail();
            bool isCorrectPassword = isValidPassword();
            
            
            return isCorrectUsername && isCorrectEmail && isCorrectPassword;

        }

        public bool isValidUsername()
        {
            bool isCorrect = true;
            var resultName = (from data in db.Users
                              where data.Username == username
                              select data.Username).ToList();
            if (string.IsNullOrWhiteSpace((string)this.username) == true)
            {
                isCorrect = SetError(labelUsername, "Username is required");
               
            }
            else if (this.username.Count(char.IsLetter) < 2)
            {
                isCorrect = SetError(labelUsername, "Username must contain at least 2 letters");
                
            }
            else if (resultName.Count() > 0)
            {
                isCorrect = SetError(labelUsername, "Username is reserved");

            }
            else
            {
               
                labelUsername.Visibility = Visibility.Hidden;
            }
            return isCorrect;
        }


        
        public bool isValidEmail()
        {
            bool isCorrect = true;
            var resultEmail = (from data in db.Users
                               where data.Email == email
                               select data.Email).ToList();
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            
            
            if (string.IsNullOrWhiteSpace((string)this.email) == true)
            {
                isCorrect = SetError(labelEmail, "Email is required");

            }
            else if (!Regex.IsMatch(email, regex, RegexOptions.IgnoreCase))
            {
                isCorrect = SetError(labelEmail, "Wrong e-mail address");
               
            }
            else if (resultEmail.Count() > 0)
            {
                isCorrect = SetError(labelEmail, "Email is reserved");

            }
            else
            {
               
                labelEmail.Visibility = Visibility.Hidden;
            }
            return isCorrect;
        }

        private bool SetError(System.Windows.Controls.Label errorLabel,string message)
        {
           
            errorLabel.Visibility = Visibility.Visible;
            errorLabel.Foreground = Brushes.Red;
            errorLabel.Content = message;
          
            return false;
        }

        public bool isValidPassword()
        {
            bool isCorrect = true;


           
            if (string.IsNullOrWhiteSpace(this.password))
            {
                isCorrect = SetError(labelPassword, "Password is required");
                
            }
            else if (this.password.ToString().Length < 8)
            {
                isCorrect = SetError(labelPassword, "Password is too short");
               

            }
            else if (this.password.ToString().Count(char.IsLetter) < 2)
            {
                isCorrect = SetError(labelPassword, "Password must contain at least 2 letters");
               

            }
            else if (this.password.ToString().Count(char.IsUpper) < 2)
            {
                isCorrect = SetError(labelPassword, "Password must contain at least 2 or more uppercase letter");

            }
            else
            {
              
                labelPassword.Visibility = Visibility.Hidden;
            }
            return isCorrect;
        }
        
    }
}
