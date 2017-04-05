using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Retire
{

	public class Report
	{
		public int Month { get; private set; }
		public int Year { get; private set; }

		[JsonProperty]
		private Dictionary<BudgetType, double> Entries = new Dictionary<BudgetType, double>();

		public Report(int month, int year)
		{
			Month = month;
			Year = year;
		}

		internal void AddExpenditure(BudgetType type, double amount)
		{
			if (Entries.ContainsKey(type))
				Entries[type] += amount;
			else
			    Entries.Add(type, amount);
		}

		public Dictionary<string, double> GetReport(bool incomeDetails = true)
		{
			var report = new Dictionary<string, double>();
			foreach (var kvp in Entries)
			{
				var amount = kvp.Value;
				var budgetCategory = new BudgetCategory(kvp.Key);
				if (budgetCategory.MainCategory == "Income" && incomeDetails && !string.IsNullOrWhiteSpace(budgetCategory.SubCategory))
					UpdateReport(report, budgetCategory.SubCategory, amount);
				else
				   UpdateReport(report, budgetCategory.MainCategory, amount);
			}
			return report;
		}

		private void UpdateReport(Dictionary<string, double> report, string category, double value)
		{
			if (report.ContainsKey(category))
				report[category] += value;
			else
				report.Add(category, value);
		}

		internal void Save(string fname)
		{
			File.WriteAllText(fname, Serialize());
		}

		internal string Serialize()
		{
			var jsonSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented
			};
			return JsonConvert.SerializeObject(this, jsonSettings);
		
		}

		public static Report DeSerialize(string savedReport)
		{
			Report report = JsonConvert.DeserializeObject<Report>(savedReport);
			return report;
		}
}
}