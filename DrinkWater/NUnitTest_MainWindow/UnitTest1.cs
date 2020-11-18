using NUnit.Framework;
using System.Collections.Generic;

namespace NUnitTest_MainWindow
{
    public class Tests
    {
        private List<KeyValuePair<string, int>> LiquidsAmount=new List<KeyValuePair<string, int>>();
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            LiquidsAmount.Add(new KeyValuePair<string, int>("Water", 700));
            DrinkWater.MainService.Add(LiquidsAmount);
            Assert.Pass();
        }
    }
}