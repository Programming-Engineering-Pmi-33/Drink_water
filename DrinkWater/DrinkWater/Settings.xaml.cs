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

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// This function initialize this window.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function sets user id and name, then starts background thread for toasts.
        /// </summary>
        /// <param name="userInfo">Argument that contains user id and name.</param>
        public void SetSessionUser(SessionUser userInfo)
        {
            user = new UserData(userInfo);
            sessionUser = userInfo;
            userData = user.GetData();
            if (userData.DisableNotifications == false)
            {
                SetTimer();
            }
        }

        /// <summary>
        /// This function set timer as background thread.
        /// </summary>
        public void SetTimer()
        {
            timer = new System.Timers.Timer();
            if (userData.NotitficationsPeriod != null)
            {
                timer.Interval = userData.NotitficationsPeriod.Value.TotalMilliseconds;
            }
            else
            {
                // 100 seconds
                timer.Interval = 100000;
            }

            timer.Elapsed += TimerFunction;
            timer.Start();
        }

        /// <summary>
        /// This function makes user parameters grid active.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Arguments.</param>
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

        /// <summary>
        /// This function makes user settings grid active.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Arguments.</param>
        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            SetUserSettingsVisibility();
            UsernameTextBox.Text = userData.Username;
            PasswordTextBox.Password = userData.Password;
            EmailTextBox.Text = userData.Email;
            if (userData.Avatar != null)
            {
                Avatar.Source = image.GetImagefromDB(userData.Avatar);
            }
        }

        /// <summary>
        /// This function redirect user to login window.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Arguments.</param>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info($"User {sessionUser.Username} logged out from system.");
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Function that displays user settings grid.
        /// </summary>
        private void SetUserSettingsVisibility()
        {
            UserSettingsGrid.Visibility = Visibility.Visible;
            if (UserParametersGrid.Visibility == Visibility.Visible)
            {
                UserParametersGrid.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Function that displays user parameters grid.
        /// </summary>
        private void SetUserParametersVisibility()
        {
            UserParametersGrid.Visibility = Visibility.Visible;
            if (UserSettingsGrid.Visibility == Visibility.Visible)
            {
                UserSettingsGrid.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// This function save parameters of active grid and notifications settings.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Arguments.</param>
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserParametersGrid.Visibility == Visibility.Visible)
            {
                ErrorLabel.Content = parametersValidation.GetUserParameterValidation(WeightTextBox.Text, HeightTextBox.Text, AgeTextBox.Text, WakeUpTextBox.Text, GoingToBedTextBox.Text);
                if (string.IsNullOrEmpty(ErrorLabel.Content.ToString()))
                {
                    long weight = Convert.ToInt32(WeightTextBox.Text);
                    long height = Convert.ToInt32(HeightTextBox.Text);
                    long age = Convert.ToInt32(AgeTextBox.Text);
                    string sex = GenderList.Text;
                    var wakeUpString = WakeUpTextBox.Text.Split(":");
                    TimeSpan wakeUp = new TimeSpan(Convert.ToInt32(wakeUpString[0]), Convert.ToInt32(wakeUpString[1]), Convert.ToInt32(wakeUpString[2]));
                    var goingToBedString = GoingToBedTextBox.Text.Split(":");
                    TimeSpan goingToBed = new TimeSpan(Convert.ToInt32(goingToBedString[0]), Convert.ToInt32(goingToBedString[1]), Convert.ToInt32(goingToBedString[2]));
                    user.SetUserParameters(weight, height, age, sex, wakeUp, goingToBed);
                }
            }

            if (UserSettingsGrid.Visibility == Visibility.Visible)
            {
                ErrorLabel.Content = settingsValidation.GetUserSettingsValidation(UsernameTextBox.Text, PasswordTextBox.Password, EmailTextBox.Text);
                if (string.IsNullOrEmpty(ErrorLabel.Content.ToString()))
                {
                    user.SetUserInformation(UsernameTextBox.Text, PasswordTextBox.Password, EmailTextBox.Text, image.GetImage());
                }
            }

            string choosenParameter = NotificationsSettings.Text;
            int customPeriod = Convert.ToInt32(CustomPeriodTextBox.Text);
            bool disableNotifications = Convert.ToBoolean(IsDisabled.Content);
            if (string.IsNullOrEmpty(ErrorLabel.Content.ToString()))
            {
                user.SetUserNotitfications(choosenParameter, customPeriod, disableNotifications);
            }
        }

        /// <summary>
        /// Function that allow user to choose photo for avatar.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            Avatar.Source = image.ChooseAvatar();
        }

        /// <summary>
        /// Function for initializing toast notifications.
        /// </summary>
        /// <param name="e">sender object.</param>
        /// <param name="x">Arguments.</param>
        private void TimerFunction(object e, EventArgs x)
        {
            ToastNotificationsClass toast = new ToastNotificationsClass();
            toast.ShowNot();
        }

        /// <summary>
        /// Redirect to profile window.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void ProfileWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileStatistics profileStatistics = new ProfileStatistics();
            profileStatistics.SetSessionUser(sessionUser);
            profileStatistics.Show();
            this.Close();
        }

        /// <summary>
        /// Redirect to main window.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.SetSessionUser(sessionUser);
            mainWindow.Show();
            this.Close();
        }

        private void NotificationsSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void GenderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserSettings_Click(sender, e);
        }
    }
}
