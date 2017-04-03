using System;
using System.Collections.Generic;

namespace Retire
{

	public class Report
	{
		public int Month { get; private set; }
		public Dictionary<BudgetType, double> Entries { get; private set; }

		public Report(int month)
		{
			Month = month;
			Entries = new Dictionary<BudgetType, double>();
		}

		internal void AddExpenditure(BudgetType type, double amount)
		{
			Entries.Add(type, amount);
		}
	}
}