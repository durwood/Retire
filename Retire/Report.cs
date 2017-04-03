using System;
using System.Collections.Generic;

namespace Retire
{

	public class Report
	{
		public int Month { get; private set; }
		private Dictionary<BudgetType, double> _entries = new Dictionary<BudgetType, double>();
		public Report(int month)
		{
			Month = month;
		}

		internal void AddExpenditure(BudgetType type, double amount)
		{
			_entries.Add(type, amount);
		}

		public Dictionary<string, double> GetReport()
		{
			var report = new Dictionary<string, double>();
			foreach (var kvp in _entries)
			{
				BudgetCategory category = new BudgetCategory(kvp.Key);
				report.Add(category.MainCategory, kvp.Value);
			}
			return report;
		}
	}
}