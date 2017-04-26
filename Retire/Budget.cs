﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace Retire
{
    class Budget
    {
        public int Year { get; private set; }
        public string User { get; private set; }

        [JsonIgnore]
        public string Title { get; private set; }
        [JsonIgnore]
        public double Total { get; private set; }

        [JsonProperty]
        List<BudgetEntry> BudgetEntries = new List<BudgetEntry>();

        Dictionary<int, double> _monthlyTotals = new Dictionary<int, double>();

		private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			Formatting = Formatting.Indented
		};

        [JsonConstructor]
        public Budget(string user, int year, List<BudgetEntry> budgetEntries)
        {
			User = user;
            Year = year > 0 ? year : DateTime.Now.Year;
            Title = this.GetTitle();
			for (int ii = 1; ii <= 12; ++ii)
				_monthlyTotals.Add(ii, 0.0);
			foreach (var entry in budgetEntries)
                AddEntry(entry);
            Console.WriteLine(_monthlyTotals);
        }

        public Budget(string user, int year) : this(user, year, new List<BudgetEntry>())
        {
		}

        public Budget(int year) : this("", year)
		{
		}

		public Budget(string user) : this(user, 0)
		{
		}

        private string GetTitle()
        {
            var title = "budget";
            if (Year != 0)
                title = $"{Year}_{title}";
            if (!string.IsNullOrWhiteSpace(User))
                title = $"{User}_{title}";
            return title;
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
            sb.AppendLine($"{Title}");
            for (int ii = 1; ii <= 12; ++ii)
            {
                var month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(ii); // .GetMonthName(ii);
                sb.AppendLine($"{month,6}: {MonthlyTotal(ii),10:C2}");
            }
            sb.AppendLine($"{"Total",6}: {Total,10:C2}");
            return sb.ToString();
        }

        internal string Serialize()
        {
            return JsonConvert.SerializeObject(this, _jsonSettings);
        }

        internal static Budget DeSerialize(string budgetString)
        {
            return JsonConvert.DeserializeObject<Budget>(budgetString, _jsonSettings);
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
    }
}