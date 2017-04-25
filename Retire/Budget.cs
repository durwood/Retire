using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace Retire
{
	class Budget
	{
		public string Title { get; set; }

		public double Total { get; private set; }

		[JsonProperty]
		List<BudgetEntry> BudgetEntries = new List<BudgetEntry>();

		Dictionary<int, double> _monthlyTotals = new Dictionary<int, double>();

		public Budget()
		{
			for (int ii = 1; ii <= 12; ++ii)
				_monthlyTotals.Add(ii, 0.0);
		}

		[JsonConstructor]
		public Budget(string title, List<BudgetEntry> budgetEntries) : this()
		{
			foreach (var entry in budgetEntries)
				AddEntry(entry);
			Title = title;
		}

		public Budget(string title) : this()
		{
			Title = title;
		}

		public void AddEntry(BudgetEntry budgetEntry)
		{
			BudgetEntries.Add(budgetEntry);
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

		internal string Serialize()
		{
			var jsonSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented
			};
			return JsonConvert.SerializeObject(this, jsonSettings);
		}

		internal static Budget DeSerialize(string budgetString)
		{
			var jsonSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented
			};
			return JsonConvert.DeserializeObject<Budget>(budgetString, jsonSettings);
		}

		internal void Save(string fname)
		{
			File.WriteAllText(fname, Serialize());
		}
}
}