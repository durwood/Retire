using System;
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
			Assert.That(_budget.Title, Is.EqualTo("TestBudget"));
			Assert.That(_budget.Total, Is.EqualTo(360.00));
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(30.00));
			Assert.That(_budget.MonthlyTotal(12), Is.EqualTo(30.00));
		}

		[Test]
		public void CanSupportMultipleBudgetEntries()
		{
			BudgetEntryMonthly anotherMonthly = new BudgetEntryMonthly(25.00, "Water Bill", BudgetType.Utilities_WaterSewerWaste);

			_budget.AddEntry(_monthlyEntry);
			_budget.AddEntry(anotherMonthly);
			Assert.That(_budget.Title, Is.EqualTo("TestBudget"));
			Assert.That(_budget.Total, Is.EqualTo((30.00 + 25.00) * 12));
			Assert.That(_budget.MonthlyTotal(3), Is.EqualTo(30.00 + 25.00));
		}

		[Test]
		public void CanSupportBiMonthlyBudgetEntries()
		{
			var _biMonthlyEntry = new BudgetEntryBiMonthly(40.00, 1, "Water Bill", BudgetType.Utilities_WaterSewerWaste);

			_budget.AddEntry(_biMonthlyEntry);
			Assert.That(_budget.Total, Is.EqualTo(40.00 * 6));
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(40.00));
			Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(0.00));
		}

		[Test]
		public void CanSupportAnnualBudgetEntries()
		{
			var anualEntry = new BudgetEntryAnnual(900.00, 1, "Mariners", BudgetType.Entertainment_SportingEvents);
			_budget.AddEntry(anualEntry);
			Assert.That(_budget.Total, Is.EqualTo(900.00));
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(900.00));
			Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(0.0));
		}

		[Test]
		public void CanSupportBiAnnualBudgetEntries()
		{
			var entry1 = new BudgetEntryBiAnnual(300, 5, "Progressive BMW", BudgetType.Auto_Insurance);
			var entry2 = new BudgetEntryBiAnnual(100, 12, "Vespa", BudgetType.Auto_Insurance);
			_budget.AddEntry(entry1);
			Assert.That(_budget.Total, Is.EqualTo(600.00));
			Assert.That(_budget.MonthlyTotal(5), Is.EqualTo(300.00));
			Assert.That(_budget.MonthlyTotal(11), Is.EqualTo(300.0));
			Assert.That(_budget.MonthlyTotal(12), Is.EqualTo(0.0));
			_budget.AddEntry(entry2);
			Assert.That(_budget.MonthlyTotal(6), Is.EqualTo(100.0));
			Assert.That(_budget.MonthlyTotal(12), Is.EqualTo(100.0));
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(0.0));
		}

		[Test]
		public void CanSupportWeeklyBudgetEntries()
		{
			var entry1 = new BudgetEntryWeekly(100, "EveryWeek", budgetType: BudgetType.Personal, period:1, start:1);
			_budget.AddEntry(entry1);
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(400));
			Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(400));
			Assert.That(_budget.Total, Is.EqualTo(5200));
		}

		[Test]
		public void CanSerializeBudget()
		{
			CreateSerializedTestBudget();
			ValidateSerializedTestBudget(_budget);
			var budgetString = _budget.Serialize();
			Console.WriteLine(budgetString);
			var budget = Budget.DeSerialize(budgetString);
			ValidateSerializedTestBudget(budget);
		}

		void ValidateSerializedTestBudget(Budget budget)
		{
			Assert.That(budget.Total, Is.EqualTo(2130.00));
			Assert.That(budget.MonthlyTotal(1), Is.EqualTo(180.00));
			Assert.That(budget.MonthlyTotal(2), Is.EqualTo(190.00));
			Assert.That(budget.MonthlyTotal(3), Is.EqualTo(180.00));
			Assert.That(budget.MonthlyTotal(4), Is.EqualTo(150.00));
			Assert.That(budget.MonthlyTotal(5), Is.EqualTo(240.00));
			Assert.That(budget.MonthlyTotal(6), Is.EqualTo(140.00));
			Assert.That(budget.MonthlyTotal(7), Is.EqualTo(190.00));
			Assert.That(budget.MonthlyTotal(8), Is.EqualTo(140.00));
			Assert.That(budget.MonthlyTotal(9), Is.EqualTo(180.00));
			Assert.That(budget.MonthlyTotal(10), Is.EqualTo(150.00));
			Assert.That(budget.MonthlyTotal(11), Is.EqualTo(240.00));
			Assert.That(budget.MonthlyTotal(12), Is.EqualTo(150.00));
		}

		void CreateSerializedTestBudget()
		{
			var entry1 = new BudgetEntryMonthly(30.00, "Gas Bill", BudgetType.Utilities_Gas);
			var entry2 = new BudgetEntryBiMonthly(40.00, 1, "Water Bill", BudgetType.Utilities_WaterSewerWaste);
			var entry3 = new BudgetEntryAnnual(50.00, 2, "Mariners", BudgetType.Entertainment_SportingEvents);
			var entry4 = new BudgetEntryBiAnnual(60.00, 5, "Progressive BMW", BudgetType.Auto_Insurance);
			var entry5 = new BudgetEntryMonthly(70.00, "Misc Utility", BudgetType.Utilities);
			var entry6 = new BudgetEntryWeekly(10, "Weekly", BudgetType.Personal);
			_budget.AddEntry(entry1);
			_budget.AddEntry(entry2);
			_budget.AddEntry(entry3);
			_budget.AddEntry(entry4);
			_budget.AddEntry(entry5);
			_budget.AddEntry(entry6);
		}
}
}
