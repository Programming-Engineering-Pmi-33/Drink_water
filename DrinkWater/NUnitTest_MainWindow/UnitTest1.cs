using DrinkWater;
using DrinkWater.LogReg;
using DrinkWater.MainServices;
using DrinkWater.ProfileStatisticsServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTest_MainWindow
{
    public class Tests
    {
        private MainService main = new MainService();

        [SetUp]
        public void Setup()
        {
            main = new MainService();
        }

        [Test]
        public void GetUser_Pass()
        {
            main.SetSessionUser(new SessionUser(1, "Mamonchik"));
            Assert.AreEqual(main.GetUser().UserId, 1);
        }

        
        [Test]
        public void Add_Pass()
        {
            using (var transaction =main.Db.Database.BeginTransaction())
            {
                main.SetSessionUser(new SessionUser(1, "Mamonchik"));
                double amount_before = 0;
                if (main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1) != null)
                {
                    amount_before = (double)main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1).Sum;
                    ValidationLiquid validation = new ValidationLiquid("Water", "100");
                    transaction.CreateSavepoint("SavePoint");
                    main.Add(validation.GetName(), validation.GetAmount());
                    double amount_after = (from water in main.Db.Statistics
                                           where water.UserIdRef == 1 && water.Date == DateTime.Now.Date && water.FluidIdRef == 1
                                           select water.FluidAmount).FirstOrDefault();
                    transaction.RollbackToSavepoint("SavePoint");
                    transaction.Dispose();
                    Assert.AreNotEqual(amount_after, amount_before); 
                    
                }
            }
        }

        [Test]
        public void Add_Fail()
        {
            main.SetSessionUser(new SessionUser(1, "Mamonchik"));
            double amount_before = (double)main.GetStatistic().GetDailyStatistics().Count;
            ValidationLiquid validation = new ValidationLiquid("Vine", "100");
            main.Add(validation.GetName(), validation.GetAmount());
            double amount_after = (double)main.GetStatistic().GetDailyStatistics().Count;
            Assert.AreEqual(amount_after, amount_before);
        }


        [Test]
        public void GetStatistic_Pass()
        {
            main.SetSessionUser(new SessionUser(1, "Mamonchik"));
            StatisticInfo statisticInfo = main.GetStatistic();
            Assert.IsTrue(statisticInfo.GetDailyStatistics().Count != 0);
        }

    }
}