namespace DrinkWater.SettingServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class UserSettingsValidation
    {
        private List<string> errorList = new List<string>();

        public UserSettingsValidation()
        {
        }

        public List<string> GetErrorList()
        {
            return errorList;
        }

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
