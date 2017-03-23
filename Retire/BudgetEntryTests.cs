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
			_monthlyEntry = new BudgetEntryMonthly(30.00, "Gas Bill", "Utility", "Gas");
		}

		[Test]
		public void CanSupportMonthlyBudgetEntries()
		{
			_budget.AddEntry(_monthlyEntry);
			Assert.That(_budget.Title, Is.EqualTo("TestBudget"));
			Assert.That(_budget.Total, Is.EqualTo(360.00));
		}

		[Test]
		public void CanRetrieveMonthlyTotal()
		{
			_budget.AddEntry(_monthlyEntry);
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(30.00));
			Assert.That(_budget.MonthlyTotal(12), Is.EqualTo(30.00));
		}

		[Test]
		public void CanSupportMultipleBudgetEntries()
		{
			BudgetEntryMonthly anotherMonthly = new BudgetEntryMonthly(25.00, "Water Bill", "Utility", "Water");

			_budget.AddEntry(_monthlyEntry);
			_budget.AddEntry(anotherMonthly);
			Assert.That(_budget.Title, Is.EqualTo("TestBudget"));
			Assert.That(_budget.Total, Is.EqualTo((30.00 + 25.00) * 12));
			Assert.That(_budget.MonthlyTotal(3), Is.EqualTo(30.00 + 25.00));
		}

		[Test]
		public void CanSupportBiMonthlyBudgetEntries()
		{
			var _biMonthlyEntry = new BudgetEntryBiMonthly(40.00, 1, "Water Bill", "Utility", "Water");

			_budget.AddEntry(_biMonthlyEntry);
			Assert.That(_budget.Total, Is.EqualTo(40.00 * 6));
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(40.00));
			Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(0.00));
		}

		[Test]
		public void CanSupportSingleMonthBudgetEntries()
		{
			var _singleMonthEntry = new BudgetEntrySpecificMonths(900.00, 1, "Mariners", "Entertainment", "SportingEvents");
			_budget.AddEntry(_singleMonthEntry);
			Assert.That(_budget.Total, Is.EqualTo(900.00));
			Assert.That(_budget.MonthlyTotal(1), Is.EqualTo(900.00));
			Assert.That(_budget.MonthlyTotal(2), Is.EqualTo(0.0));
		}

	}
}
