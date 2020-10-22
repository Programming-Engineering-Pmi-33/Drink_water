using System.Linq;
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
        public Registration()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBox1.Text;
            this.email = textBox2.Text;
            this.password = textBox3.Text;
            if (isUnique(this.username, this.email, this.password))
            {
                if (IsValid())
                {
                    if (password == textBox4.Text)
                    {

                        Users user = new Users(username, email, password);
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

        private bool isUnique(string username, string email, string password)
        {
            var resultName = (from data in db.Users
                              where data.Username == username
                              select data.Username).ToList();
            if (resultName.Count() > 0)
            {
                MessageBox.Show("Username is reserved", "Validation Error");
                return false;
            }
            var resultEmail = (from data in db.Users
                               where data.Email == email
                               select data.Email).ToList();
            if (resultEmail.Count() > 0)
            {
                MessageBox.Show("Email is reserved", "Validation Error");
                return false;
            }
            var resultPassword = (from data in db.Users
                                  where data.Password == password
                                  select data.Password).ToList();
            if (resultPassword.Count() > 0)
            {
                MessageBox.Show("Password is reserved", "Validation Error");
                return false;
            }
            return true;
        }
        private bool IsValid()
        {

            if (string.IsNullOrWhiteSpace((string)this.username) == true)
            {
                MessageBox.Show("Username is required", "Validation Error");
                return false;
            }
            if (this.username.Count(char.IsLetter) < 2)
            {
                MessageBox.Show("Username must contain at least 2 letters", "Validation Error");
                return false;
            }
            if (string.IsNullOrWhiteSpace((string)this.email) == true)
            {
                MessageBox.Show("Email is required", "Validation Error");
                return false;
            }
            if (!this.email.ToString().Contains("@gmail.com"))
            {
                MessageBox.Show("Wrong e-mail address", "Validation Error");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.password))
            {
                MessageBox.Show("Password is required", "Validation Error");
                return false;
            }
            if (this.password.ToString().Length < 8)
            {
                MessageBox.Show("Password is too short", "Validation Error");
                return false;
            }
            if (this.password.ToString().Count(char.IsLetter) < 2)
            {
                MessageBox.Show("Password must contain at least 2 letters", "Validation Error");
                return false;
            }
            if (this.password.ToString().Count(char.IsUpper) < 2)
            {
                MessageBox.Show("Password must contain at least 2 or more uppercase letter", "Validation Error");
                return false;
            }
            return true;
        }
    }
}
