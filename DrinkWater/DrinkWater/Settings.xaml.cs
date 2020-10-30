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
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Threading;
using Windows.System;

namespace DrinkWater
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        static private SessionUser sessionUser = new SessionUser();
        static private dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        static byte[] ImageArray;
        System.Timers.Timer timer;
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
            timer = new System.Timers.Timer();
            if (userData.NotitficationsPeriod != null)
            {
                timer.Interval = userData.NotitficationsPeriod.Value.TotalMilliseconds;
            }
            else
            {
                timer.Interval = 300000;
            }
            timer.Elapsed += TimerFunction;
            timer.Start();
        }
        private void UserParameters_Click(object sender, RoutedEventArgs e)
        {
            SetUserParametersVisibility();
            WeightTextBox.Text = userData.Weight.ToString();
            HeightTextBox.Text = userData.Height.ToString();
            AgeTextBox.Text = userData.Age.ToString();
            if (userData.Sex != null & userData.Sex == "Male")
            {
                GenderList.SelectedIndex = 0;
            }
            if (userData.Sex != null & userData.Sex == "Female")
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
            if (userData.Avatar != null)
            {
                var memoryStream = new MemoryStream(userData.Avatar);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                Avatar.Source = bitmap;
            }
            

        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Dispose();
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
        protected void SetUserSettingsVisibility()
        {

            UserSettingsGrid.Visibility = Visibility.Visible;
            if (UserParametersGrid.Visibility == Visibility.Visible)
            {
                UserParametersGrid.Visibility = Visibility.Hidden;
            }
        }
        protected void SetUserParametersVisibility()
        {
            UserParametersGrid.Visibility = Visibility.Visible;
            if (UserSettingsGrid.Visibility == Visibility.Visible)
            {
                UserSettingsGrid.Visibility = Visibility.Hidden;
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserParametersGrid.Visibility == Visibility.Visible)
            {
                userData.Weight = (long)Convert.ToInt32(WeightTextBox.Text);
                userData.Height = (long)Convert.ToInt32(HeightTextBox.Text);
                userData.Age = (long)Convert.ToInt32(AgeTextBox.Text);
                userData.Sex = GenderList.Text;
                var WakeUpString = WakeUpTextBox.Text.Split(":");
                userData.WakeUp = new TimeSpan(Convert.ToInt32(WakeUpString[0]), Convert.ToInt32(WakeUpString[1]), Convert.ToInt32(WakeUpString[2]));//доробити
                var GoingToBedString = GoingToBedTextBox.Text.Split(":");
                userData.GoingToBed = new TimeSpan(Convert.ToInt32(GoingToBedString[0]), Convert.ToInt32(GoingToBedString[1]), Convert.ToInt32(GoingToBedString[2]));
            }
            if (UserSettingsGrid.Visibility == Visibility.Visible)
            {
                userData.Username = UsernameTextBox.Text;
                userData.Password = PasswordTextBox.Text;
                userData.Email = EmailTextBox.Text;
                userData.Avatar = ImageArray;
            }
            string str = NotificationsSettings.Text;
            if(str.Contains("Custom"))
            {
                userData.NotitficationsPeriod = new TimeSpan(Convert.ToInt32(CustomPeriodTextBox.Text), 0, 0);
            }
            switch(NotificationsSettings.Text)
            {
                case "Every hour":
                    userData.NotitficationsPeriod = new TimeSpan(1,0,0);
                    break;

                case "Every day":
                    userData.NotitficationsPeriod = new TimeSpan(24, 0, 0);
                    break;

                default:
                    break;

            }
            userData.DisableNotifications = Convert.ToBoolean(IsDisabled.Content);
            db.SaveChanges();
        }

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";

            if (dlg.ShowDialog() == true)
            {
                Bitmap bitmap = new Bitmap(dlg.FileName);
                Avatar.Source = ConvertBitmap(bitmap);
            }

        }
        public BitmapImage ConvertBitmap(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ImageArray = ms.ToArray();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public void TimerFunction(object e, EventArgs x)
        {
            ToastNotificationsClass toast = new ToastNotificationsClass();
            toast.ShowNot();
        }
    }
}
