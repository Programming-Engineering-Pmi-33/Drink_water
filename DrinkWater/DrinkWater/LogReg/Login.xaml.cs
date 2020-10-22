using System;
using System.Windows;
using System.Linq;

namespace DrinkWater.LogReg
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login :Window
    {

        public dfkg9ojh16b4rdContext db=new dfkg9ojh16b4rdContext();
        private string username;
        private string password;
        public Login()
        {
            
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration registration=new Registration();
            registration.Show();
            this.Close();

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.username = textBox1.Text;
                this.password = textBox2.Text;
                var result = (from data in db.Users
                              where data.Username == username
                              select data.Password).FirstOrDefault();

            if (password == result)
                {
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
                    MessageBox.Show("incorrect data of user,try again.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("incorrect data of user,try again.");
            }

        }

       
    }
}
