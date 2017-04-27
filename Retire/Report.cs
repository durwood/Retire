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
        public string User { get; private set; }
        [JsonIgnore]
        public string Title { get; private set; }
        [JsonIgnore]
        public double Expenses { get; internal set; }
        [JsonIgnore]
        public double Income { get; internal set; }

        [JsonProperty]
		private Dictionary<BudgetType, double> Entries = new Dictionary<BudgetType, double>();

		public Report(int month, int year) : this(month, year, "")
		{
		}

        [JsonConstructor]
        public Report(int month, int year, string user)
		{
			Month = month;
			Year = year;
            User = user;
            Title = GetTitle();
        }

		private string GetTitle()
		{
            var title = $"{Year}_{Month:D2}_report";
			if (!string.IsNullOrWhiteSpace(User))
				title = $"{User}_{title}";
			return title;
		}

		internal void AddExpenditure(BudgetType type, double amount)
		{
            if (BudgetCategory.IsIncome(type))
                Income += amount;
            else
                Expenses += amount;

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

                if (budgetCategory.MainCategory == "Income" && incomeDetails)
					UpdateReport(report, amount, budgetCategory.MainCategory, budgetCategory.SubCategory);
				else
				   UpdateReport(report, amount, budgetCategory.MainCategory);
			}
			return report;
		}

		private void UpdateReport(Dictionary<string, double> report, double value, string category, string subCategory = "")
		{
            var key = category;
            if (!string.IsNullOrWhiteSpace(subCategory))
                key = $"{category}_{subCategory}";

            if (report.ContainsKey(key))
                    report[key] += value;
                else
                    report.Add(key, value);
		}

		internal void Save(string location = "")
		{
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!string.IsNullOrWhiteSpace(location))
                directory = Path.Combine(directory, location);
            
            var fname = $"{Title}.json";
            var fullpath = Path.Combine(directory, fname);
			File.WriteAllText(fullpath, Serialize());
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