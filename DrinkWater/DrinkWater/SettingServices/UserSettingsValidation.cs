namespace DrinkWater.SettingServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class for user settings validation.
    /// </summary>
    public class UserSettingsValidation
    {
        private List<string> errorList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsValidation"/> class.
        /// </summary>
        public UserSettingsValidation()
        {
        }

        /// <summary>
        /// Getter for errorList property.
        /// </summary>
        /// <returns>List of labels where data was not valid.</returns>
        public List<string> GetErrorList()
        {
            return errorList;
        }

        /// <summary>
        /// This function checks inputed data and show where it was not valid.
        /// </summary>
        /// <param name="username">User name value.</param>
        /// <param name="password">User password value.</param>
        /// <param name="email">User email value.</param>
        /// <returns>String of errors.</returns>
        public string GetUserSettingsValidation(string username, string password, string email)
        {
            string validationResult = string.Empty;
            string usernameValidation = UsernameValidation(username);
            string passwordValidation = PasswordValidatoin(password);
            string emailValidation = EmailValidation(email);
            if (usernameValidation != null)
            {
                validationResult += usernameValidation;
                errorList.Add("username");
            }

            if (passwordValidation != null)
            {
                validationResult += passwordValidation;
                errorList.Add("password");
            }

            if (emailValidation != null)
            {
                validationResult += emailValidation;
                errorList.Add("email");
            }

            return validationResult;
        }

        private string UsernameValidation(string username)
        {
            string validationResult = string.Empty;
            if (string.IsNullOrWhiteSpace(username))
            {
                validationResult += "Name required\n";
                return validationResult;
            }

            if (username.Length < 2)
            {
                validationResult += "Name must contain at least 2 letter\n";
            }

            return validationResult;
        }

        private string PasswordValidatoin(string password)
        {
            string validationResult = string.Empty;
            if (string.IsNullOrWhiteSpace(password))
            {
                validationResult = "Password required\n";
                return validationResult;
            }

            if (password.Count(char.IsLetter) < 2)
            {
                validationResult += "Password must contain at least 2 letter\n";
            }

            if (password.Length < 8)
            {
                validationResult += "Password is too short\n";
            }

            if (password.Count(char.IsUpper) < 1)
            {
                validationResult += "Password must contain at least 1 or more Uppercase letter\n";
            }

            return validationResult;
        }

        private string EmailValidation(string email)
        {
            string validationResult = string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                validationResult += "Password required\n";
                return validationResult;
            }

            if (!email.Contains('@'))
            {
                validationResult += "Email is not valid. @ required.\n";
            }

            return validationResult;
        }
    }
}
