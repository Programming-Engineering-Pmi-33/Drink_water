namespace DrinkWater
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using DrinkWater.LogReg;
    using DrinkWater.SettingServices;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for Settings.xaml.
    /// </summary>
    public partial class Settings : Window
    {
        private static SessionUser sessionUser = new SessionUser();
        private static ImageHandler image = new ImageHandler();
        private static UserParametersValidation parametersValidation = new UserParametersValidation();
        private static UserSettingsValidation settingsValidation = new UserSettingsValidation();
        private static User userData;
        private static UserData user;
        private System.Timers.Timer timer;

        public Settings()
        {
            InitializeComponent();
        }

        public void GetSessionUser(SessionUser userInfo)
        {
            Logger.InitLogger();

            Logger.Log.Info("Ура робе!");
            user = new UserData(userInfo);
            sessionUser = userInfo;
            userData = user.GetData();
            timer = new System.Timers.Timer();
            if (userData.NotitficationsPeriod != null)
            {
                timer.Interval = 100000;
            }
            else
            {
                timer.Interval = 5000;
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
                Avatar.Source = image.GetImagefromDB(userData.Avatar);
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

        private void SetUserSettingsVisibility()
        {
            UserSettingsGrid.Visibility = Visibility.Visible;
            if (UserParametersGrid.Visibility == Visibility.Visible)
            {
                UserParametersGrid.Visibility = Visibility.Hidden;
            }
        }

        private void SetUserParametersVisibility()
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
                ErrorLabel.Content = parametersValidation.GetUserParameterValidation(WeightTextBox.Text, WeightTextBox.Text, AgeTextBox.Text, WakeUpTextBox.Text, GoingToBedTextBox.Text);
                long weight = Convert.ToInt32(WeightTextBox.Text);
                long height = Convert.ToInt32(WeightTextBox.Text);
                long age = Convert.ToInt32(AgeTextBox.Text);
                string sex = GenderList.Text;
                var wakeUpString = WakeUpTextBox.Text.Split(":");
                TimeSpan wakeUp = new TimeSpan(Convert.ToInt32(wakeUpString[0]), Convert.ToInt32(wakeUpString[1]), Convert.ToInt32(wakeUpString[2]));
                var goingToBedString = GoingToBedTextBox.Text.Split(":");
                TimeSpan goingToBed = new TimeSpan(Convert.ToInt32(goingToBedString[0]), Convert.ToInt32(goingToBedString[1]), Convert.ToInt32(goingToBedString[2]));
                if (ErrorLabel.Content == "")
                {
                    user.SetUserParameters(weight, height, age, sex, wakeUp, goingToBed);
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }

            if (UserSettingsGrid.Visibility == Visibility.Visible)
            {
                user.SetUserInformation(UsernameTextBox.Text, PasswordTextBox.Text, EmailTextBox.Text, image.GetImage());
            }

            string choosenParameter = NotificationsSettings.Text;
            int customPeriod = Convert.ToInt32(CustomPeriodTextBox.Text);
            bool disableNotifications = Convert.ToBoolean(IsDisabled.Content);
            if (ErrorLabel.Content == "")
            {
                user.SetUserNotitfications(choosenParameter, customPeriod, disableNotifications);
            }
        }

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";

            if (dlg.ShowDialog() == true)
            {
                Bitmap bitmap = new Bitmap(dlg.FileName);
                Avatar.Source = image.ConvertBitmap(bitmap);
            }
        }

        private void TimerFunction(object e, EventArgs x)
        {
            ToastNotificationsClass toast = new ToastNotificationsClass();
            toast.ShowNot();
        }

        private void ProfileWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileStatistics profileStatistics = new ProfileStatistics();
            profileStatistics.GetSessionUser(sessionUser);
            profileStatistics.Show();
            this.Close();
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
