namespace NUnitTest_MainWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DrinkWater;
    using DrinkWater.LogReg;
    using DrinkWater.MainServices;
    using DrinkWater.ProfileStatisticsServices;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Npgsql;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for checking the functionality of the main service.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Create MainService object.
        /// </summary>
        private MainService main = new MainService();

        /// <summary>
        /// Setup method.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.main = new MainService();
        }

        /// <summary>
        /// Test for checking method that returns information about session user.
        /// </summary>
        [Test]
        public void GetUser_Pass()
        {
            this.main.SetSessionUser(new SessionUser(1, "Mamonchik"));
            Assert.AreEqual(this.main.GetUser().UserId, 1);
        }

        /// <summary>
        /// Check the correct method for adding liquid for valid data.
        /// </summary>
        [Test]
        public void Add_Pass()
        {
            using (var transaction = this.main.db.Database.BeginTransaction())
            {
                this.main.SetSessionUser(new SessionUser(1, "Mamonchik"));
                double amount_before = 0;
                if (this.main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1) != null)
                {
                    amount_before = (double)this.main.GetStatistic().GetDailyStatistics().Find(x => x.FluidIdRef == 1).Sum;
                    ValidationLiquid validation = new ValidationLiquid("Water", "100");
                    transaction.CreateSavepoint("SavePoint");
                    this.main.Add(validation.GetName(), validation.GetAmount());
                    double amount_after = (from water in this.main.db.Statistics
                                           where water.UserIdRef == 1 && water.Date == DateTime.Now.Date && water.FluidIdRef == 1
                                           select water.FluidAmount).FirstOrDefault();
                    transaction.RollbackToSavepoint("SavePoint");
                    Assert.AreNotEqual(amount_after, amount_before);
                }
            }
        }

        /// <summary>
        /// Check the correct method for adding the liquid that didn`t exist in the database.
        /// </summary>
        [Test]
        public void Add_Fail()
        {
            this.main.SetSessionUser(new SessionUser(1, "Mamonchik"));
            double amount_before = (double)this.main.GetStatistic().GetDailyStatistics().Count;
            ValidationLiquid validation = new ValidationLiquid("Vine", "100");
            this.main.Add(validation.GetName(), validation.GetAmount());
            double amount_after = (double)this.main.GetStatistic().GetDailyStatistics().Count;
            Assert.AreEqual(amount_after, amount_before);
        }

        /// <summary>
        /// Test for checking method GetStatistic that return statistics of the current user.
        /// </summary>
        [Test]
        public void GetStatistic_Pass()
        {
            this.main.SetSessionUser(new SessionUser(1, "Mamonchik"));
            StatisticInfo statisticInfo = this.main.GetStatistic();
            Assert.IsTrue(statisticInfo.GetDailyStatistics().Count != 0);
        }
    }
}