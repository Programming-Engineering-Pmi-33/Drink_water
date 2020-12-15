namespace DrinkWater.Services
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Announces ValidationService сlass.
    /// </summary>
    public class ValidationService
    {
        private const string EMAILREGEX = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private readonly UsersService usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationService"/> class.
        /// </summary>
        /// <param name="usersService">user service instance.</param>
        public ValidationService(UsersService usersService)
        {
            this.usersService = usersService;
        }

        /// <summary>
        /// Validates username, email and password and sets errors to labels if any.
        /// </summary>
        /// <param name="labelUsername">error label for username.</param>
        /// <param name="username">username value.</param>
        /// <param name="labelEmail">error label for email.</param>
        /// <param name="email">email value.</param>
        /// <param name="labelPassword">error label for password.</param>
        /// <param name="password">password value.</param>
        /// <returns>bool value.</returns>
        public bool IsValid(Label labelUsername, string username, Label labelEmail, string email, Label labelPassword, string password)
        {
            bool isCorrectUsername = IsValidUsername(labelUsername, username);
            bool isCorrectEmail = IsValidEmail(labelEmail, email);
            bool isCorrectPassword = IsValidPassword(labelPassword, password);

            return isCorrectUsername && isCorrectEmail && isCorrectPassword;
        }

        /// <summary>
        /// Displayes a message on the label.
        /// </summary>
        /// <param name="errorLabel">label.</param>
        /// <param name="message">text.</param>
        public static void SetError(Label errorLabel, string message)
        {
            errorLabel.Visibility = Visibility.Visible;
            errorLabel.Foreground = Brushes.Red;
            errorLabel.Content = message;
        }

        /// <summary>
        ///  Checks correctness of username.
        /// </summary>
        /// <param name="labelUsername">username label.</param>
        /// <param name="username">username value.</param>
        /// <returns>bool value.</returns>
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

        /// <summary>
        /// Checks correctness of email.
        /// </summary>
        /// <param name="labelEmail">email label.</param>
        /// <param name="email">email value.</param>
        /// <returns>bool value.</returns>
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

        /// <summary>
        /// Checks correctness of password.
        /// </summary>
        /// <param name="labelPassword">password label.</param>
        /// <param name="password">password value.</param>
        /// <returns>bool value.</returns>
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