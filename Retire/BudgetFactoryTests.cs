using NUnit.Framework;

namespace Retire
{
	[TestFixture]
	public class BudgetFactoryTests
	{
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
	}
}
