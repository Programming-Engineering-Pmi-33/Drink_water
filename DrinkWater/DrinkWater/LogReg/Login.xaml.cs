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

        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                this.username = textBoxUsername.Text;
                this.password = textBoxPassword.Text;
                var result = (from data in db.Users
                              where data.Username == username
                              select data.Password).FirstOrDefault();

                if (password == result)
                {
                    var id = (from user in db.Users
                              where user.Password == password && user.Username == username
                              select user.UserId).FirstOrDefault();

                    SessionUser sessionUser = new SessionUser((long)id, username);
                    MessageBox.Show("it works.");
                    MainWindow Main = new MainWindow();
                    Main.main.GetSessionUser(sessionUser);
                    Main.Show();
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

        private void buttonCreateNewAccount_Click(object sender, RoutedEventArgs e)
        {

            Registration registration = new Registration();
            registration.Show();
            this.Close();

        }
    }
}
