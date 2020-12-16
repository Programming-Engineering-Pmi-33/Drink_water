namespace DrinkWater.SettingServices
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class for user parameters validation.
    /// </summary>
    public class UserParametersValidation
    {
        private List<string> errorList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserParametersValidation"/> class.
        /// </summary>
        public UserParametersValidation()
        {
        }

        /// <summary>
        /// Getter for <c>errorList</c> property.
        /// </summary>
        /// <returns>List in which column data was not valid.</returns>
        public List<string> GetErrorList()
        {
            return errorList;
        }

        /// <summary>
        ///  This function validate parameters inputed by user.
        /// </summary>
        /// <param name="weight">User weight value.</param>
        /// <param name="height">User height value.</param>
        /// <param name="age">User age value.</param>
        /// <param name="wakeUp">Value when user wakes up.</param>
        /// <param name="goingToBed">Value of time whe user goes to bed.</param>
        /// <returns>String of errors.</returns>
        public string GetUserParameterValidation(string weight, string height, string age, string wakeUp, string goingToBed)
        {
            string parametersValidations = string.Empty;
            string weightValidation = WeightValidation(weight);
            string heightValidation = HeightValidation(height);
            string ageValidation = AgeValidation(age);
            string timeValidation = TimeValidation(wakeUp, goingToBed);
            if (weightValidation != null)
            {
                parametersValidations += weightValidation;
                errorList.Add("weight");
            }

            if (heightValidation != null)
            {
                parametersValidations += heightValidation;
                errorList.Add("height");
            }

            if (ageValidation != null)
            {
                parametersValidations += ageValidation;
                errorList.Add("age");
            }

            if (timeValidation != null)
            {
                parametersValidations += timeValidation;
                errorList.Add("time");
            }

            return parametersValidations;
        }

        private string WeightValidation(string weight)
        {
            if (string.IsNullOrEmpty(weight))
            {
                return "Field cannot be empty ";
            }

            if (!int.TryParse(weight, out int res))
            {
                return "Invalid input. Must be a number ";
            }

            if (Convert.ToInt32(weight) <= 0)
            {
                return "Weight must be non-negative ";
            }

            return string.Empty;
        }

        private string HeightValidation(string height)
        {
            if (string.IsNullOrEmpty(height))
            {
                return "Field cannot be empty ";
            }

            if (!int.TryParse(height, out int res))
            {
                return "Invalid input. Must be a number ";
            }

            if (Convert.ToInt32(height) <= 0)
            {
                return "Height must be non-negative or zero ";
            }

            return string.Empty;
        }

        private string AgeValidation(string age)
        {
            if (string.IsNullOrEmpty(age))
            {
                return "Field cannot be empty ";
            }

            if (!int.TryParse(age, out int res))
            {
                return "Invalid input. Must be a number ";
            }

            if (Convert.ToInt32(age) <= 0)
            {
                return "Age must be non-negative or zero ";
            }

            return string.Empty;
        }

        private string TimeValidation(string wakeUp, string goingToBed)
        {
            string validationResult = string.Empty;
            if (string.IsNullOrWhiteSpace(wakeUp) || string.IsNullOrWhiteSpace(goingToBed))
            {
                return "Fields cannot be empty ";
            }

            var wake = wakeUp.Split(':');
            var bed = goingToBed.Split(':');
            if (wake.Length < 3 || bed.Length < 3)
            {
                return "Invalid input.\n Input must be like '00:00:00' ";
            }

            if (Convert.ToInt32(wake[0]) > 24 || Convert.ToInt32(bed[0]) > 24 || Convert.ToInt32(wake[0]) < 0 || Convert.ToInt32(bed[0]) < 0)
            {
                return "Inputed hours are invalid ";
            }

            for (int i = 1; i <= 2; i++)
            {
                if (Convert.ToInt32(wake[i]) > 60 || Convert.ToInt32(wake[i]) < 0 || Convert.ToInt32(bed[i]) > 60 || Convert.ToInt32(bed[i]) < 0)
                {
                    if (i == 1)
                    {
                        return "Inputed minutes are invalid ";
                    }

                    if (i == 2)
                    {
                        return "Inputed seconds are invalid ";
                    }
                }
            }

            return validationResult;
        }
    }
}
