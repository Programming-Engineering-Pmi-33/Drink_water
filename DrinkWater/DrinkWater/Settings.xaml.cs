using DrinkWater.LogReg;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DrinkWater
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        static private SessionUser sessionUser = new SessionUser();
        static private dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        static Users userData;
        public Settings()
        {
            InitializeComponent();
        }

        public void GetSessionUser(SessionUser user)
        {
            sessionUser = user;
            userData = (from searchingUser in db.Users
                        where searchingUser.UserId == sessionUser.UserId
                        select searchingUser).FirstOrDefault();
        }
        private void UserParameters_Click(object sender, RoutedEventArgs e)
        {
            SetUserParametersVisibility();
            WeightTextBox.Text = userData.Weight.ToString();
            HeightTextBox.Text = userData.Height.ToString();
            AgeTextBox.Text = userData.Age.ToString();
            if (userData.Gender != null & userData.Gender=="Male")
            {
                GenderList.SelectedIndex = 0;
            }
            if (userData.Gender != null & userData.Gender == "Female")
            {
                GenderList.SelectedIndex = 1;
            }
            WakeUpTextBox.Text = userData.WakeUp.ToString();
            GoingToBedTextBox.Text = userData.GoingToBed.ToString();
        }

        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            SetUserSettingsVisibility();
            UsernameTextBox.Text = userData.Username;
            PasswordTextBox.Text = userData.Password;
            EmailTextBox.Text = userData.Email;
            

        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
        protected void SetUserSettingsVisibility()
        {

            UsernameLabel.Visibility = Visibility.Visible;
            PasswordLabel.Visibility = Visibility.Visible;
            EmailLabel.Visibility = Visibility.Visible;
            UsernameTextBox.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Visible;
            EmailTextBox.Visibility = Visibility.Visible;
            if(WeightLabel.Visibility == Visibility.Visible)
            {
                WeightLabel.Visibility = Visibility.Hidden;
                WeightTextBox.Visibility = Visibility.Hidden;
                HeightLabel.Visibility = Visibility.Hidden;
                HeightTextBox.Visibility = Visibility.Hidden;
                AgeLabel.Visibility = Visibility.Hidden;
                AgeTextBox.Visibility = Visibility.Hidden;
                GenderLabel.Visibility = Visibility.Hidden;
                GenderList.Visibility = Visibility.Hidden;
                ActivityTimeLabel.Visibility = Visibility.Hidden;
                WakeUpTextBox.Visibility = Visibility.Hidden;
                GoingToBedTextBox.Visibility = Visibility.Hidden;
            }
        }
        protected void SetUserParametersVisibility()
        {
            WeightLabel.Visibility = Visibility.Visible;
            WeightTextBox.Visibility = Visibility.Visible;
            HeightLabel.Visibility = Visibility.Visible;
            HeightTextBox.Visibility = Visibility.Visible;
            AgeLabel.Visibility = Visibility.Visible;
            AgeTextBox.Visibility = Visibility.Visible;
            GenderLabel.Visibility = Visibility.Visible;
            GenderList.Visibility = Visibility.Visible;
            ActivityTimeLabel.Visibility = Visibility.Visible;
            WakeUpTextBox.Visibility = Visibility.Visible;
            GoingToBedTextBox.Visibility = Visibility.Visible;
            if(UsernameLabel.Visibility == Visibility.Visible)
            {
                UsernameLabel.Visibility = Visibility.Hidden;
                PasswordLabel.Visibility = Visibility.Hidden;
                EmailLabel.Visibility = Visibility.Hidden;
                UsernameTextBox.Visibility = Visibility.Hidden;
                PasswordTextBox.Visibility = Visibility.Hidden;
                EmailTextBox.Visibility = Visibility.Hidden;
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if(WeightLabel.Visibility == Visibility.Visible)
            {
                userData.Weight = (long)Convert.ToInt32(WeightTextBox.Text);
                userData.Height = (long)Convert.ToInt32(HeightTextBox.Text);
                userData.Age = (long)Convert.ToInt32(AgeTextBox.Text);
                userData.Gender = GenderList.Text;
                userData.WakeUp = new TimeSpan(1, 1, 1);//доробити
                userData.GoingToBed = new TimeSpan(2, 5, 6);
                db.SaveChanges();
            }   
            if(UsernameLabel.Visibility== Visibility.Visible)
            {
                userData.Username = UsernameTextBox.Text;
                userData.Password = PasswordTextBox.Text;
                userData.Email = EmailTextBox.Text;
                db.SaveChanges();
            }
        }
    }
}
