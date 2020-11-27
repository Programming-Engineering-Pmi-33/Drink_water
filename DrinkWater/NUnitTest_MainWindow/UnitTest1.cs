using DrinkWater;
using DrinkWater.LogReg;
using DrinkWater.MainServices;
using DrinkWater.ProfileStatisticsServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NUnitTest_MainWindow
{
    public class Tests
    {
         private MainService main = new MainService();
        


        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void GetUser_Pass()
        {
            main.GetSessionUser(new SessionUser(long.Parse("1"), "Mamonchik"));
            Assert.AreEqual(main.GetUser().UserId, long.Parse("1"));
        }
        
        [Test]
        public void Add_Pass()
        {
            main.GetSessionUser(new SessionUser(long.Parse("1"), "Mamonchik"));
            double amount_before = (double)main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1).Sum;
            ValidationLiquid validation = new ValidationLiquid("Water", "100");
            main.Add(validation.GetName(), validation.GetAmount());
            double amount_after = (double)main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1).Sum;
            Assert.AreNotEqual(amount_after, amount_before);
        }

        [Test]
        public void Add_Fail()
        {
            main.GetSessionUser(new SessionUser(1, "Mamonchik"));
            double amount_before = (double)main.GetStatistic().GetDailyStatistics().Count;
            ValidationLiquid validation = new ValidationLiquid("Vine", "100");
            main.Add(validation.GetName(), validation.GetAmount());
            double amount_after = (double)main.GetStatistic().GetDailyStatistics().Count;
            Assert.AreEqual(amount_after, amount_before);
        }

        [Test]
        public void Add1_Pass()
        {
            main.GetSessionUser(new SessionUser(1, "Mamonchik"));
            double amount_before = (double)main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1).Sum;
            ValidationLiquid validation = new ValidationLiquid("Water", "-100");
            main.Add(validation.GetName(), validation.GetAmount()); 
            double amount_after = (double)main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1).Sum;
            Assert.AreEqual(amount_before, amount_after);
        }

        [Test]
        public void GetStatistic_Pass()
        {
            main.GetSessionUser(new SessionUser(1, "Mamonchik"));
            StatisticInfo statisticInfo = main.GetStatistic();
            Assert.IsTrue(statisticInfo.GetDailyStatistics().Count != 0);
        }

    }
}