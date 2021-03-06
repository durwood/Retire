﻿using NUnit.Framework;

namespace Retire
{

    [TestFixture]
	public class BudgetFactoryTests
	{
        [SetUp]
        public void SetUp()
        {
            BudgetFactory.CreateBudget(2017, "AnyOldUser");
        }

		[Test]
		public void CreateBudgetEntryTests()
		{
			var budgetEntry = BudgetFactory.CreateBudgetEntry("Annual", "Personal", "10", "annualExample", "2");
			Assert.That(budgetEntry, Is.TypeOf(typeof(BudgetEntryAnnual)));

			budgetEntry = BudgetFactory.CreateBudgetEntry("BiAnnual", "Personal", "10", "biAnnualExample", "2");
			Assert.That(budgetEntry, Is.TypeOf(typeof(BudgetEntryBiAnnual)));

			budgetEntry = BudgetFactory.CreateBudgetEntry("Monthly", "Personal", "10", "monthlyExample");
			Assert.That(budgetEntry, Is.TypeOf(typeof(BudgetEntryMonthly)));

			budgetEntry = BudgetFactory.CreateBudgetEntry("BiMonthly", "Personal", "10", "biMonthlyExample", "2");
			Assert.That(budgetEntry, Is.TypeOf(typeof(BudgetEntryBiMonthly)));

			budgetEntry = BudgetFactory.CreateBudgetEntry("Weekly", "Personal", "10", "weeklyExample", "Jan 4", "5", "2");
			Assert.That(budgetEntry, Is.TypeOf(typeof(BudgetEntryWeekly)));

			budgetEntry = BudgetFactory.CreateBudgetEntry("Daily", "Personal", "10", "dailyExample", "Jan 1");
			Assert.That(budgetEntry, Is.TypeOf(typeof(BudgetEntryDaily)));
		}

        [Test]
        public void CanCreateEmptyBudget()
        {
			BudgetFactory.CreateBudget(2017, "AnyOldUser");
            var budget = BudgetFactory.GetBudget();
            Assert.That(budget, Is.TypeOf(typeof(Budget)));
            Assert.That(budget.TotalNet, Is.EqualTo(0.0));

		}

        [Test]
        public void CanCreateMonthlyBudget()
        {
			BudgetFactory.CreateMonthly(BudgetType.Home_Mortgage, "Mortgage", 2000);
            Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(12 * 2000.0));
		}

		[Test]
		public void CanCreateAnnualBudget()
		{
			BudgetFactory.CreateAnnual(BudgetType.Auto_Insurance, "Vespa", 300.00, 4);
            Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(300.0));
		}

		[Test]
		public void CanCreateBiAnnualBudget()
		{
			BudgetFactory.CreateBiAnnual(BudgetType.Auto_Insurance, "BMW", 400.00, 4);
            Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(2 * 400.0));
		}

		[Test]
		public void CanCreateWeeklyBudget()
		{
			BudgetFactory.CreateWeekly(BudgetType.Personal, "Dues", 10.00);
			Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(52 * 10.0));

			BudgetFactory.CreateBudget(2017);
			BudgetFactory.CreateWeekly(BudgetType.Personal, "Dues", 10.00, period: 2);
			Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(26 * 10.0));

			BudgetFactory.CreateBudget(2017);
			BudgetFactory.CreateWeekly(BudgetType.Personal, "Dues", 10.00, period: 1, start: "Jan 10, 2017");
			Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(51 * 10.0));

			BudgetFactory.CreateBudget(2017);
			BudgetFactory.CreateWeekly(BudgetType.Personal, "Dues", 10.00, period:1, start:"Jan 1, 2017", max: 51);
            Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(51 * 10.0));
		}

		[Test]
		public void CanCreateDailyBudget()
		{
			BudgetFactory.CreateDaily(BudgetType.Personal, "Food", 10.00);
			var budget = BudgetFactory.GetBudget();
			Assert.That(budget.TotalExpenses, Is.EqualTo(365 * 10.0));

            BudgetFactory.CreateBudget(2017);
            BudgetFactory.CreateDaily(BudgetType.Personal, "Food", 10.00, "Jan 2, 2017");
            Assert.That(BudgetFactory.GetBudget().TotalExpenses, Is.EqualTo(364.0 * 10.0));

		}
	}
}
