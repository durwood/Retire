using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"{this.Title}");
			for (int ii=1; ii <= 12; ++ii)
			{
				var month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(ii); // .GetMonthName(ii);
				sb.AppendLine($"{month,6}: {MonthlyTotal(ii),10:C2}");
			}
			sb.AppendLine($"{"Total",6}: {Total,10:C2}");
			return sb.ToString();
		}
	}
}