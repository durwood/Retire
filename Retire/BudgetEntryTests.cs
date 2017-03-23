using System;
using NUnit.Framework;

namespace Retire
{
	[TestFixture]
	public class BudgetTests
	{
		private Budget _budget;

		[SetUp]
		public void SetUp()
		{
			_budget = new Budget("TestBudget");
		}

		[Test]
		public void CanSupportMonthlyBudgetEntries()
		{
			//Budget budget = new Budget("TestBudget");
			BudgetEntryMonthly budgetEntry = new BudgetEntryMonthly(30.00, "Gas Bill", "Utility", "Gas");
			_budget.AddEntry(budgetEntry);
			Assert.That(_budget.Title, Is.EqualTo("TestBudget"));
			Assert.That(_budget.Total, Is.EqualTo(360.00));
		}

		[Test]
		public void CanRetrieveMonthlyTotal()
		{
			
		}

	}
}
