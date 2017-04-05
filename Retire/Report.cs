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

		public Dictionary<string, double> GetReport()
		{
			var report = new Dictionary<string, double>();
			foreach (var kvp in Entries)
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
			File.WriteAllText(fname, Serialize());
		}

		internal string Serialize()
		{
			var jsonSettings = new JsonSerializerSettings
			{
				//TypeNameHandling = TypeNameHandling.All,
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