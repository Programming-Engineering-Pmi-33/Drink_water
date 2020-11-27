namespace DrinkWater.Services
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ValidationService
    {
        private const string EMAILREGEX = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private readonly UsersService usersService;

        // constructor
        public ValidationService(UsersService usersService)
        {
            this.usersService = usersService;
        }

        // validation of username, email and password
        public bool IsValid(Label labelUsername, string username, Label labelEmail, string email, Label labelPassword, string password)
        {
            bool isCorrectUsername = IsValidUsername(labelUsername, username);
            bool isCorrectEmail = IsValidEmail(labelEmail, email);
            bool isCorrectPassword = IsValidPassword(labelPassword, password);

            return isCorrectUsername && isCorrectEmail && isCorrectPassword;
        }

        // displaying a message on the form
        public static void SetError(Label errorLabel, string message)
        {
            errorLabel.Visibility = Visibility.Visible;
            errorLabel.Foreground = Brushes.Red;
            errorLabel.Content = message;
        }

        // checking correctness of username
        private bool IsValidUsername(Label labelUsername, string username)
        {
            bool isCorrect = false;

            if (string.IsNullOrWhiteSpace((string)username) == true)
            {
                SetError(labelUsername, "Username is required");
            }
            else if (username.Count(char.IsLetter) < 2)
            {
                SetError(labelUsername, "Username must contain at least 2 letters");
            }
            else if (usersService.UsernameExists(username))
            {
                SetError(labelUsername, "Username is reserved");
            }
            else
            {
                isCorrect = true;
                labelUsername.Visibility = Visibility.Hidden;
            }

            return isCorrect;
        }

        // checking correctness of email
        private bool IsValidEmail(Label labelEmail, string email)
        {
            bool isCorrect = false;

            if (string.IsNullOrWhiteSpace(email) == true)
            {
                SetError(labelEmail, "Email is required");
            }
            else if (!Regex.IsMatch(email, EMAILREGEX, RegexOptions.IgnoreCase))
            {
                SetError(labelEmail, "Wrong e-mail address");
            }
            else if (usersService.EmailExists(email))
            {
                SetError(labelEmail, "Email is reserved");
            }
            else
            {
                isCorrect = true;
                labelEmail.Visibility = Visibility.Hidden;
            }

            return isCorrect;
        }

        // checking correctness of password
        private bool IsValidPassword(Label labelPassword, string password)
        {
            bool isCorrect = false;

            if (string.IsNullOrWhiteSpace(password))
            {
                SetError(labelPassword, "Password is required");
            }
            else if (password.ToString().Length < 8)
            {
                SetError(labelPassword, "Password is too short");
            }
            else if (password.ToString().Count(char.IsLetter) < 2)
            {
                SetError(labelPassword, "Password must contain at least 2 letters");
            }
            else if (password.ToString().Count(char.IsUpper) < 2)
            {
                SetError(labelPassword, "Password must contain at least 2 or more uppercase letter");
            }
            else
            {
                isCorrect = true;
                labelPassword.Visibility = Visibility.Hidden;
            }

            return isCorrect;
        }
    }
}