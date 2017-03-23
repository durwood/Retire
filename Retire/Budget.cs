using System;
using System.Collections.Generic;

namespace Retire
{
	class Budget
	{
		public string Title { get; private set; }
		public double Total { get; private set; }
		List<BudgetEntry> _budgetEntries = new List<BudgetEntry>();

		public Budget(string title)
		{
			this.Title = title;
		}

		public void AddEntry(BudgetEntry budgetEntry)
		{
			_budgetEntries.Add(budgetEntry);
			Total = Total + budgetEntry.AnnualizedAmount;
		}

		public double MonthlyTotal(int month)
		{
			double total = 0.0;
			foreach (var entry in _budgetEntries)
			{
				if (entry is BudgetEntryMonthly)
				{
					total += ((BudgetEntryMonthly)entry).Amount;
				}
			}
			return total;
		}
	}
}