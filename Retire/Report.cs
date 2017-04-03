using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

		internal void Save(string fname)
		{
			var reportString = Serialize();
			using (var file = new StreamWriter(fname))
			{
				file.WriteLine(reportString);
				file.Close();
			}
			return;
		}

		internal string Serialize()
		{
			var sb = new StringBuilder();
			sb.AppendLine(Month.ToString());
			foreach (var kvp in GetReport())
			{
				if (Math.Abs(kvp.Value) > 0.005)
				{
					var amount = kvp.Value.ToString();
					sb.AppendLine($"{kvp.Key} {amount}");
				}
			}
				
			return sb.ToString();;
		}

		public static Report DeSerialize(string savedReport)
		{
			Report report = null;
			int month = 0;
			using (StringReader reader = new StringReader(savedReport))
			{
				string line = string.Empty;
				do
				{
					line = reader.ReadLine();
					if (line != null)
					{
						if (month == 0)
						{
							month = int.Parse(line);
							report = new Report(month);
						}
						else
						{
							var components = line.Split(' ');
							var budgetType = BudgetCategoryFactory.DeSerialize(components[0]);
							var amount = double.Parse(components[1]);
							report.AddExpenditure(budgetType, amount);
						}
						// do something with the line
					}

				} while (line != null);
			}

			return report;
		}
}
}