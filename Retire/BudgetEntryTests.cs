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
	}
}
