using DrinkWater;
using DrinkWater.LogReg;
using DrinkWater.SettingServices;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Windows.System.UserProfile;
using Xunit;

namespace Setting.Tests
{
    public class UnitTest1
    { 
        [Theory]
        [InlineData("-2", "88", "100", "22:00:00", "11:00:00", "Weight must be non-negative")]
        [InlineData("2", "-88", "100", "22:00:00", "11:00:00", "Height must be non-negative")]
        [InlineData("2", "88", "-100", "22:00:00", "11:00:00", "Age must be non-negative")]
        [InlineData("2", "88", "100", "44:00:00", "11:00:00", "Inputed hours are invalid.")]
        [InlineData("2", "88", "100", "22:00:00", "222:00:00", "Inputed hours are invalid.")]
        [InlineData("2", "88", "100", "22:00:00", "11:00:00", "")]
        public void UserParametersTestTheoryMethod(string weight, string height, string age, string wakeUp, string goingBed, string res)
        {
            UserParametersValidation parametersValidation = new UserParametersValidation();
            string validationResult = parametersValidation.GetUserParameterValidation(weight, height, age, wakeUp, goingBed);
            Assert.Equal(validationResult, res);
        }
        
        [Theory]
        [InlineData("A", "Aa123456", "honolulu@gmail.com", "Name must contain at least 2 letter\n")]
        [InlineData("", "Aa123456", "honolulu@gmail.com", "Name required\n")]
        [InlineData("ABBA", "", "honolulu@gmail.com", "Password required\n")]
        [InlineData("ABBA", "A11111111111", "honolulu@gmail.com", "Password must contain at least 2 letter\n")]
        public void UserSettingsTestTheoryMethod(string username, string password, string email, string res)
        {
            UserSettingsValidation settingsValidation = new UserSettingsValidation();
            string validationResult = settingsValidation.GetUserSettingsValidation(username, password, email);
            Assert.Equal(validationResult, res);
        }
    }
}
