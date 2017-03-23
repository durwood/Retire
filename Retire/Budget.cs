using System;

namespace Retire
{
	class Budget
	{
		public string Title { get; private set; }
		BudgetEntry budgetEntry;
		public double Total { get; private set; }

		public Budget(string title)
		{
			this.Title = title;
		}

		public void AddEntry(BudgetEntry budgetEntry)
		{
			this.budgetEntry = budgetEntry;
			Total = Total + budgetEntry.AnnualizedAmount;
		}
	}
}