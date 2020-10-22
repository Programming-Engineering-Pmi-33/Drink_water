using System.Windows;
using System.Linq;
using System.Net.Sockets;
using System;

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
            this.username = textBox1.Text;
            this.password = textBox2.Text;


            var result = (from data in db.Users
                          where data.Username == username
                          select data.Password).FirstOrDefault();
            if (isInDatabase(result))
            {
                if (password == result)
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
    }
}
