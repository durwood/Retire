﻿using System;

namespace Retire
{
	// TODO:
	// DONE Support weekly budget entries
	// DONE Convert to using JSON for serizlization
	// DONE Add year to report heading i.e 2/2017
	// DONE Allow for Income details to be saved
	// DONE Add Income to budget (support weekly income, too?)
    // Create Daily Entry?
	// Consider how to do budget category mapping and overall flow from Mint
	// Allow for budgeted amounts to be included in report
	//

	class MainClass
	{
        public class ArgParser
        {
            private string[] _args;
            public int Year;
            public string User;

            public ArgParser(string[] args)
            {
                _args = args;
            }

            public bool Parse()
            {
                if (_args.Length == 2)
                {
                    User = _args[0];
					Year = int.Parse(_args[1]);
					return true;
				}
				PrintUsage();
				return false;
            }

            private void PrintUsage()
            {
                Console.WriteLine("Retire <user> <year>");
            }

        }

        public static void Main(string[] args)
        {
            // PrintSpecialFolders();
            var argParser = new ArgParser(args);
            if (argParser.Parse())
            {
                var year = argParser.Year;
                var user = argParser.User;

                var budget = CreateBudget(year, user);
                budget.Save();

				var month = GetMonth();
				var report = DoMonthlyReport(month, year, user);
                report.Save();
            }
        }

		public static void PrintSpecialFolders()
		{
			foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
			{
				Console.WriteLine($"{folder.ToString()} : {Environment.GetFolderPath(folder)}");
			}
		}

		public static Budget CreateBudget(int year, string user)
		{
            BudgetFactory.CreateBudget(year, user);
			BudgetFactory.CreateMonthly(BudgetType.Home_Mortgage, "Mortgage", 2000);
			BudgetFactory.CreateMonthly(BudgetType.Utilities_InternetCable, "Comcast", 242.00);
			BudgetFactory.CreateMonthly(BudgetType.Utilities_Gas, "Gas", 90.00);
			BudgetFactory.CreateBiMonthly(BudgetType.Utilities_WaterSewerWaste, "SPU", 190.00, 1);
			BudgetFactory.CreateBiMonthly(BudgetType.Utilities_Electricity, "SCL", 80.00, 1);

			BudgetFactory.CreateBiAnnual(BudgetType.Auto_Insurance, "BMW", 463.00, 4);
			BudgetFactory.CreateAnnual(BudgetType.Auto_Insurance, "Vespa", 389.00, 4);
			BudgetFactory.CreateMonthly(BudgetType.Auto_Gas, "Gas", 60.00);

			BudgetFactory.CreateMonthly(BudgetType.Medical_Insurance, "Regence", 749.00);

			BudgetFactory.CreateDaily(BudgetType.Personal_Dining, "Food", 50.00);

			BudgetFactory.CreateMonthly(BudgetType.Digital_Music, "EchoDot", 3.99);
			BudgetFactory.CreateAnnual(BudgetType.Digital_Music, "AmazonCloud", 24.99, 2);
			BudgetFactory.CreateMonthly(BudgetType.Digital_Subscription, "Github", 7.00);
			BudgetFactory.CreateAnnual(BudgetType.Digital_Subscription, "AmazonPrime", 99.00, 12);
			BudgetFactory.CreateAnnual(BudgetType.Shopping_Subscription, "Costco", 55.00, 3);
			BudgetFactory.CreateMonthly(BudgetType.Digital_Movies, "Netflix", 10.95);
			BudgetFactory.CreateMonthly(BudgetType.Digital_Movies, "CBS", 7.95);
			BudgetFactory.CreateAnnual(BudgetType.Digital_Subscription, "Lastpass", 13.61, 4);
			BudgetFactory.CreateWeekly(BudgetType.Personal, "Haircut", 60.00, period:6, start:"Jan 11");
		    BudgetFactory.CreateWeekly(BudgetType.Income_Unemployment, "WA Unemployment", 681.00, start:"Feb 11", period:1, max:26);

			BudgetFactory.CreateAnnual(BudgetType.Entertainment_SportingEvents, "Mariners", 900.00, 1);
			BudgetFactory.CreateAnnual(BudgetType.Entertainment_Movies, "SIFF", 600.00, 12);

			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Christmas", 1600.00, 12);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Michelle", 150, 3);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Chris", 150, 1);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Thea", 150, 10);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Dan", 100, 8);

			var budget = BudgetFactory.GetBudget();
			Console.WriteLine(budget);

			return budget;
		}

		public static Report DoMonthlyReport(int month, int year, string user)
		{
			var report = new Report(month, year, user);

			AddIncome(report);
			AddExpenses(report);

			foreach (var reportEntry in report.GetReport())
				Console.WriteLine(reportEntry.ToString());
			
			return report;
		}

		static string _prompt = "> ";

		private static void AddIncome(Report report)
		{
			Console.WriteLine("When prompted, enter income...");
			var incomeCategories = BudgetCategoryFactory.GetIncomeCategories();
			foreach (var category in incomeCategories)
			{
				var prompt = string.IsNullOrWhiteSpace(category) ? "Unspecified Income" : category;
				var amount = GetPromptedAmount(prompt);
				var budgetType = BudgetCategoryFactory.GetBudgetType("Income", category);
				report.AddExpenditure(budgetType, amount);
			}
		}

		private static void AddExpenses(Report report)
		{
			Console.WriteLine("When prompted, enter aggregated expenses...");
			var expenseCategories = BudgetCategoryFactory.GetExpenseCategories();
			foreach (var category in expenseCategories)
			{
				var amount = GetPromptedAmount(category);
				var budgetType = BudgetCategoryFactory.GetBudgetType(category, "");
				report.AddExpenditure(budgetType, amount);
			}
		}

		private static int GetMonth()
		{
			int month;
			Console.Write($"Enter integer value for month{_prompt}");
			do
			{
				var input = Console.ReadLine();
				if (int.TryParse(input, out month) && month > 0 && month < 13)
					break;
				Console.Write($"Invalid Month. Try again{_prompt}");
			} while (true);
			return month;
		}

		private static double GetPromptedAmount(string prompt)
		{
			double amount = 0.0;
			var fullPrompt = $"{prompt}{_prompt}";
			do
			{
				Console.Write(fullPrompt);
				var input = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(input))
					break;
				if (double.TryParse(input, out amount))
					break;
				Console.WriteLine("Invalid Input. Try again.");
			} while (true);
			return amount;
		}
	}
}
