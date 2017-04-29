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
        public double TotalNet { get; private set; }
        [JsonIgnore]
        public double TotalExpenses { get; private set; }
		[JsonIgnore]
		public double TotalIncome { get; private set; }

		[JsonProperty]
        List<BudgetEntry> BudgetEntries = new List<BudgetEntry>();

		Dictionary<int, double> _monthlyTotals = new Dictionary<int, double>{
			{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12, 0}};
        Dictionary<int, double> _monthlyExpenses = new Dictionary<int, double> {
			{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12, 0}};
		Dictionary<int, double> _monthlyIncome = new Dictionary<int, double> {
			{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12, 0}};


		private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			Formatting = Formatting.Indented
		};

        [JsonConstructor]
        public Budget(int year, string user, List<BudgetEntry> budgetEntries)
        {
			User = user;
            Year = year > 0 ? year : DateTime.Now.Year;
            Title = this.GetTitle();
			foreach (var entry in budgetEntries)
                AddEntry(entry);
            Console.WriteLine(_monthlyTotals);
        }

        public Budget(int year, string user) : this(year, user, new List<BudgetEntry>())
        {
		}

        public Budget(int year) : this(year, "")
		{
		}

		public Budget(string user) : this(0, user)
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
                if (BudgetCategory.IsIncome(budgetEntry.BudgetType))
                {
                    _monthlyIncome[ii] += monthlyEntry;
                    TotalIncome += monthlyEntry;
                    _monthlyTotals[ii] += monthlyEntry;
                    TotalNet += monthlyEntry;
                }
                else
                {
                    _monthlyExpenses[ii] += monthlyEntry;
                    TotalExpenses += monthlyEntry;
                    _monthlyTotals[ii] -= monthlyEntry;
                    TotalNet -= monthlyEntry;
                }
            }
        }

        public double MonthlyTotal(int month)
        {
            return _monthlyTotals[month];
        }

        public double MonthlyExpenses(int month)
        {
            return _monthlyExpenses[month];
        }

        public double MonthlyIncome(int month)
        {
            return _monthlyIncome[month];
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
            sb.AppendLine($"{"Total",6}: {TotalNet,10:C2}");
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