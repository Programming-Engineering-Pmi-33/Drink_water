using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                MessageBox.Show("passwords don`t match");

            }
           
        }
    }
}
