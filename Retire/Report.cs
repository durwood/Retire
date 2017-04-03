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
			if (_entries.ContainsKey(type))
				_entries[type] += amount;
			else
			    _entries.Add(type, amount);
		}

		public Dictionary<string, double> GetReport()
		{
			var report = new Dictionary<string, double>();
			foreach (var kvp in _entries)
			{
				var mainCategory = new BudgetCategory(kvp.Key).MainCategory;
				if (report.ContainsKey(mainCategory))
					report[mainCategory] += kvp.Value;
				else
				    report.Add(mainCategory, kvp.Value);
			}
			return report;
		}
	}
}