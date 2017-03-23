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
		Dictionary<int, double> _monthlyTotals = new Dictionary<int, double>();

		public Budget(string title)
		{
			this.Title = title;
			for (int ii = 1; ii <= 12; ++ii)
				_monthlyTotals.Add(ii, 0.0);
		}

		public void AddEntry(BudgetEntry budgetEntry)
		{
			_budgetEntries.Add(budgetEntry);
			for (int ii = 1; ii <= 12; ++ii)
			{
				var monthlyEntry = budgetEntry.GetMonthEntry(ii);
				_monthlyTotals[ii] += monthlyEntry;
				Total += monthlyEntry;
			}
		}

		public double MonthlyTotal(int month)
		{
			return _monthlyTotals[month];
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