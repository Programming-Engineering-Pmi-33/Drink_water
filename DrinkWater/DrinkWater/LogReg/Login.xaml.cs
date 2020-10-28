using System.Windows;
using System.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

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
                          where data.Username == username
                          select data.Password).FirstOrDefault();
            if (isInDatabase(result))
            {
                var salt = (from data in db.Users
                              where data.Username == username
                              select data.Salt).FirstOrDefault();
                string decodedPassword = ComputeSaltedHash(this.password, int.Parse(salt));
                if (decodedPassword==result)
                {
                    var id = (from user in db.Users
                              where user.Password == password
                              select user.UserId).FirstOrDefault();
                    SessionUser sessionUser = new SessionUser((long)id, username);
                    MessageBox.Show("it works.");
                }
                else
                {
                    MessageBox.Show("Incorrect password,try again", "Validation Error");
                }
            }
            else
            {
                MessageBox.Show("No such username in database", "Validation Error");
            }
        }

        public string ComputeSaltedHash(string password, int salt)
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

        //private string DecodePassword(string encodedPassword)
        //{
        //    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //    System.Text.Decoder utf8Decode = encoder.GetDecoder();
        //    byte[] decodedBytes = Convert.FromBase64String(encodedPassword);
        //    int charCount = utf8Decode.GetCharCount(decodedBytes, 0, decodedBytes.Length);
        //    char[] decodedChar = new char[charCount];
        //    utf8Decode.GetChars(decodedBytes, 0, decodedBytes.Length, decodedChar, 0);
        //    string result = new String(decodedChar);
        //    return result;
        //}

      
    }
}
