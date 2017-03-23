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

	}
}
