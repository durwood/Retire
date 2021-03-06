﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Retire
{
    [TestFixture]
	public class BudgetTests
	{
		private Budget _budget;
		private BudgetEntryMonthly _monthlyEntry;

		[SetUp]
		public void SetUp()
		{
			_budget = new Budget("TestBudget");
			_monthlyEntry = new BudgetEntryMonthly(30.00, "Gas Bill", BudgetType.Utilities_Gas);
		}

		[Test]
		public void CanSupportMonthlyBudgetEntries()
		{
			_budget.AddEntry(_monthlyEntry);
			Assert.That(_budget.TotalExpenses, Is.EqualTo(360.00));
			Assert.That(_budget.MonthlyExpenses(1), Is.EqualTo(30.00));
			Assert.That(_budget.MonthlyExpenses(12), Is.EqualTo(30.00));
		}

		[Test]
		public void CanSupportMultipleBudgetEntries()
		{
			BudgetEntryMonthly anotherMonthly = new BudgetEntryMonthly(25.00, "Water Bill", BudgetType.Utilities_WaterSewerWaste);

			_budget.AddEntry(_monthlyEntry);
			_budget.AddEntry(anotherMonthly);
			Assert.That(_budget.TotalExpenses, Is.EqualTo((30.00 + 25.00) * 12));
			Assert.That(_budget.MonthlyExpenses(3), Is.EqualTo(30.00 + 25.00));
		}

		[Test]
		public void CanSupportBiMonthlyBudgetEntries()
		{
			var _biMonthlyEntry = new BudgetEntryBiMonthly(40.00, 1, "Water Bill", BudgetType.Utilities_WaterSewerWaste);

			_budget.AddEntry(_biMonthlyEntry);
			Assert.That(_budget.TotalExpenses, Is.EqualTo(40.00 * 6));
			Assert.That(_budget.MonthlyExpenses(1), Is.EqualTo(40.00));
			Assert.That(_budget.MonthlyExpenses(2), Is.EqualTo(0.00));
		}

		[Test]
		public void CanSupportAnnualBudgetEntries()
		{
			var anualEntry = new BudgetEntryAnnual(900.00, 1, "Mariners", BudgetType.Entertainment_SportingEvents);
			_budget.AddEntry(anualEntry);
			Assert.That(_budget.TotalExpenses, Is.EqualTo(900.00));
			Assert.That(_budget.MonthlyExpenses(1), Is.EqualTo(900.00));
			Assert.That(_budget.MonthlyExpenses(2), Is.EqualTo(0.0));
		}

		[Test]
		public void CanSupportBiAnnualBudgetEntries()
		{
			var entry1 = new BudgetEntryBiAnnual(300, 5, "Progressive BMW", BudgetType.Auto_Insurance);
			var entry2 = new BudgetEntryBiAnnual(100, 12, "Vespa", BudgetType.Auto_Insurance);
			_budget.AddEntry(entry1);
			Assert.That(_budget.TotalExpenses, Is.EqualTo(600.00));
			Assert.That(_budget.MonthlyExpenses(5), Is.EqualTo(300.00));
			Assert.That(_budget.MonthlyExpenses(11), Is.EqualTo(300.0));
			Assert.That(_budget.MonthlyExpenses(12), Is.EqualTo(0.0));
			_budget.AddEntry(entry2);
			Assert.That(_budget.MonthlyExpenses(6), Is.EqualTo(100.0));
			Assert.That(_budget.MonthlyExpenses(12), Is.EqualTo(100.0));
			Assert.That(_budget.MonthlyExpenses(1), Is.EqualTo(0.0));
		}

        [Test]
        public void CanSupportDailyBudgetEntries()
        {
            var entry1 = new BudgetEntryDaily(amount: 50, label: "EveryDay", budgetType: BudgetType.Personal, start: "Jan 1");
            _budget.AddEntry(entry1);
            Assert.That(_budget.MonthlyExpenses(1), Is.EqualTo(31 * 50.0));
            Assert.That(_budget.TotalExpenses, Is.EqualTo(365 * 50.0));
        }

		[Test]
		public void CanSupportWeeklyBudgetEntries()
		{
			var entry1 = new BudgetEntryWeekly(100, "EveryWeek", budgetType: BudgetType.Personal, period:1, start:"Jan 4");
			_budget.AddEntry(entry1);
			Assert.That(_budget.MonthlyExpenses(1), Is.EqualTo(400));
			Assert.That(_budget.MonthlyExpenses(2), Is.EqualTo(400));
			Assert.That(_budget.TotalExpenses, Is.EqualTo(5200));
		}

		[Test]
		public void CanSupportSimpleWeeklyBudgetEntriesWithLimit()
		{
			var entry1 = new BudgetEntryWeekly(681, "WA Unemployment", BudgetType.Income_Unemployment, start: "Feb 11", period: 1, max: 1);
			_budget.AddEntry(entry1);
			Assert.That(_budget.MonthlyIncome(1), Is.EqualTo(0.0));
			Assert.That(_budget.MonthlyIncome(2), Is.EqualTo(681.00));
			Assert.That(_budget.MonthlyIncome(3), Is.EqualTo(0.0));
			Assert.That(_budget.TotalIncome, Is.EqualTo(681.00));
		}

		[Test]
		public void CanSupportWeeklyBudgetEntriesWithLimit()
		{
			var entry1 = new BudgetEntryWeekly(681, "WA Unemployment", BudgetType.Income_Unemployment, start: "Feb 11", period: 1, max: 26);
			_budget.AddEntry(entry1);
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(0.0));
			Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(3 * 681.00));
			Assert.That(_budget.MonthlyTotal(3), Is.EqualTo(4 * 681.00));
			Assert.That(_budget.MonthlyTotal(4), Is.EqualTo(5 * 681.00));
			Assert.That(_budget.MonthlyTotal(5), Is.EqualTo(4 * 681.00));
			Assert.That(_budget.MonthlyTotal(6), Is.EqualTo(4 * 681.00));
			Assert.That(_budget.MonthlyTotal(7), Is.EqualTo(5 * 681.00));
			Assert.That(_budget.MonthlyTotal(8), Is.EqualTo(1 * 681.00));
			Assert.That(_budget.MonthlyTotal(9), Is.EqualTo(0.0));
			Assert.That(_budget.TotalIncome, Is.EqualTo(17706));
		}



		[Test]
		public void CanJsonSerializeBudgetEntries()
		{
			var jsonSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			};

			List<BudgetEntry> input = new List<BudgetEntry>();
			input.Add(new BudgetEntryMonthly(400, "Foo", BudgetType.Auto_Gas));
			input.Add(new BudgetEntryBiMonthly(100, 1, "FooBar", BudgetType.Auto_Insurance));
			input.Add(new BudgetEntryAnnual(50, 2, "Feb", BudgetType.Entertainment_SportingEvents));
			input.Add(new BudgetEntryBiAnnual(30.0, 3, "Mar_Sep", BudgetType.Auto_Insurance));
			input.Add(new BudgetEntryWeekly(300, "FooBar", BudgetType.Personal, 6, "Jan 4"));
            input.Add(new BudgetEntryDaily(50, "LifeBlood", BudgetType.Personal, "Jan 1"));

			var json = JsonConvert.SerializeObject(input, Formatting.Indented,jsonSettings );
			Console.WriteLine(json);
			var result = JsonConvert.DeserializeObject<List<BudgetEntry>>(json, jsonSettings );

			Assert.That(result[0], Is.TypeOf(typeof(BudgetEntryMonthly)));
			Assert.That(result[1], Is.TypeOf(typeof(BudgetEntryBiMonthly)));
			Assert.That(result[2], Is.TypeOf(typeof(BudgetEntryAnnual)));
		    Assert.That(result[3], Is.TypeOf(typeof(BudgetEntryBiAnnual)));
			Assert.That(result[4], Is.TypeOf(typeof(BudgetEntryWeekly)));
            Assert.That(result[5], Is.TypeOf(typeof(BudgetEntryDaily)));
		}

		[Test]
		public void CanSerializeBudget()
		{
			CreateSerializedTestBudget();

			var budgetString = _budget.Serialize();
			//Console.WriteLine(budgetString);
			var budget = Budget.DeSerialize(budgetString);

			ValidateSerializedTestBudget(budget);
		}

		void ValidateSerializedTestBudget(Budget budget)
		{
			Assert.That(budget.TotalNet, Is.EqualTo(_budget.TotalNet));
            for (int month = 1; month <= 12; ++month)
                Assert.That(budget.MonthlyTotal(month), Is.EqualTo(_budget.MonthlyTotal(month)));
		}

		void CreateSerializedTestBudget()
		{
            var entries = new List<BudgetEntry>();
            entries.Add(new BudgetEntryMonthly(30.00, "Gas Bill", BudgetType.Utilities_Gas));
			entries.Add(new BudgetEntryBiMonthly(40.00, 1, "Water Bill", BudgetType.Utilities_WaterSewerWaste));
			entries.Add(new BudgetEntryAnnual(50.00, 2, "Mariners", BudgetType.Entertainment_SportingEvents));
			entries.Add(new BudgetEntryBiAnnual(60.00, 5, "Progressive BMW", BudgetType.Auto_Insurance));
			entries.Add(new BudgetEntryMonthly(70.00, "Misc Utility", BudgetType.Utilities));
			entries.Add(new BudgetEntryWeekly(10, "Weekly", BudgetType.Personal, 1, "Jan 4"));
            entries.Add(new BudgetEntryDaily(50, "Daily", BudgetType.Personal, "Jan 1"));
            foreach (var entry in entries)
			    _budget.AddEntry(entry);
		}

		[Test]
		public void WeeklySerialization()
		{
			var entry1 = new BudgetEntryWeekly(10, "Weekly", BudgetType.Personal, 1, "Jan 4");
			_budget.AddEntry(entry1);
			var budget = _budget;

			Assert.That(budget.TotalExpenses, Is.EqualTo(520.00));
			Assert.That(budget.MonthlyExpenses(1), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(2), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(3), Is.EqualTo(50.00));
			Assert.That(budget.MonthlyExpenses(4), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(5), Is.EqualTo(50.00));
			Assert.That(budget.MonthlyExpenses(6), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(7), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(8), Is.EqualTo(50.00));
			Assert.That(budget.MonthlyExpenses(9), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(10), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(11), Is.EqualTo(50.00));
			Assert.That(budget.MonthlyExpenses(12), Is.EqualTo(40.00));
		}

        [Test]
        public void WeeklySerializationSupportsMax()
        {
			var entry1 = new BudgetEntryWeekly(10, "Weekly", BudgetType.Personal, 1, "Jan 4", max:6);
			_budget.AddEntry(entry1);
			var budget = _budget;

			Assert.That(budget.TotalExpenses, Is.EqualTo(60.00));
			Assert.That(budget.MonthlyExpenses(1), Is.EqualTo(40.00));
			Assert.That(budget.MonthlyExpenses(2), Is.EqualTo(20.00));
        }

        [Test]
        public void WeeklySerializeWorksForAllYears()
        {
			var entry1 = new BudgetEntryWeekly(10, "Weekly", BudgetType.Personal, 1, "Oct 12, 1955");
			_budget.AddEntry(entry1);
			var budget = _budget;

			Assert.That(budget.TotalExpenses, Is.EqualTo(120.00));
			Assert.That(budget.MonthlyExpenses(9), Is.EqualTo(0.00));
			Assert.That(budget.MonthlyExpenses(10), Is.EqualTo(30.00));
			Assert.That(budget.MonthlyExpenses(11), Is.EqualTo(50.00));
			Assert.That(budget.MonthlyExpenses(12), Is.EqualTo(40.00));
            Assert.That(budget.MonthlyExpenses(1), Is.EqualTo(0.00));
		}

        [Test]
        public void CanGetExpenseAndIncomeTotal()
        {
            _budget.AddEntry(new BudgetEntryMonthly(2000.0, "mortgage", BudgetType.Home_Mortgage));
            _budget.AddEntry(new BudgetEntryMonthly(200.0, "tv", BudgetType.Utilities_InternetCable));
            _budget.AddEntry(new BudgetEntryMonthly(3000.0, "income", BudgetType.Income_Salary));
            _budget.AddEntry(new BudgetEntryMonthly(500.0, "airbnb", BudgetType.Income_Airbnb));
            const double monthlyIncome = 3000 + 500;
            const double monthlyExpenses = 2000 + 200;
            const double monthlyNet = monthlyIncome - monthlyExpenses;
            Assert.That(_budget.MonthlyExpenses(2), Is.EqualTo(monthlyExpenses));
            Assert.That(_budget.TotalExpenses, Is.EqualTo(12.0 * monthlyExpenses));
            Assert.That(_budget.MonthlyIncome(2), Is.EqualTo(monthlyIncome));
            Assert.That(_budget.TotalIncome, Is.EqualTo(12.0 * monthlyIncome));
            Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(monthlyNet));
            Assert.That(_budget.TotalNet, Is.EqualTo(12.0 * monthlyNet));
        }

    }
}
