using System;

namespace Retire
{
	// TODO:
	// DONE Support weekly budget entries
	// DONE Convert to using JSON for serizlization
	// DONE Add year to report heading i.e 2/2017
	// DONE Allow for Income details to be saved
	// Add Income to budget (support weekly income, too?)
	// Consider how to do budget category mapping and overall flow from Mint
	// Allow for budgeted amounts to be included in report
	//

	class MainClass
	{
		public static void Main(string[] args)
		{
			// PrintSpecialFolders();
			var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var budgetPath = $"{docPath}/SaveMeBudget.txt";
			var reportPath = $"{docPath}/SaveMeReport.txt";

			var budget = CreateBudget("2017 Budget");
			budget.Save(budgetPath);
			Console.WriteLine(budget);

			var report = DoMonthlyReport();
			report.Save(reportPath);
		}

		public static void PrintSpecialFolders()
		{
			foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
			{
				Console.WriteLine($"{folder.ToString()} : {Environment.GetFolderPath(folder)}");
			}
		}

		public static Budget CreateBudget(string title)
		{
			BudgetFactory.CreateMonthly(BudgetType.Home_Mortgage, "Mortgage", 2000);
			BudgetFactory.CreateMonthly(BudgetType.Utilities_InternetCable, "Comcast", 242.00);
			BudgetFactory.CreateMonthly(BudgetType.Utilities_Gas, "Gas", 90.00);
			BudgetFactory.CreateBiMonthly(BudgetType.Utilities_WaterSewerWaste, "SPU", 190.00, 1);
			BudgetFactory.CreateBiMonthly(BudgetType.Utilities_Electricity, "SCL", 80.00, 1);

			BudgetFactory.CreateBiAnnual(BudgetType.Auto_Insurance, "BMW", 463.00, 4);
			BudgetFactory.CreateAnnual(BudgetType.Auto_Insurance, "Vespa", 389.00, 4);
			BudgetFactory.CreateMonthly(BudgetType.Auto_Gas, "Gas", 60.00);

			BudgetFactory.CreateMonthly(BudgetType.Medical_Insurance, "Regence", 749.00);

			// TBD: BudgetFactory.CreateDaily(BudgetType.Dining, "Food", 50.00);

			BudgetFactory.CreateMonthly(BudgetType.Digital_Music, "EchoDot", 3.99);
			BudgetFactory.CreateAnnual(BudgetType.Digital_Music, "AmazonCloud", 24.99, 2);
			BudgetFactory.CreateMonthly(BudgetType.Digital_Subscription, "Github", 7.00);
			BudgetFactory.CreateAnnual(BudgetType.Digital_Subscription, "AmazonPrime", 99.00, 12);
			BudgetFactory.CreateAnnual(BudgetType.Shopping_Subscription, "Costco", 55.00, 3);
			BudgetFactory.CreateMonthly(BudgetType.Digital_Movies, "Netflix", 10.95);
			BudgetFactory.CreateMonthly(BudgetType.Digital_Movies, "CBS", 7.95);
			BudgetFactory.CreateAnnual(BudgetType.Digital_Subscription, "Lastpass", 13.61, 4);
			BudgetFactory.CreateWeekly(BudgetType.Personal, "Haircut", 60.00, period:6, start:"Jan 11");
			//BudgetFactory.CreateWeekly(BudgetType.Income_Unemployment, "WA Unemployment", 681.00, start:"Feb 11", period:1, count:26);

			BudgetFactory.CreateAnnual(BudgetType.Entertainment_SportingEvents, "Mariners", 900.00, 1);
			BudgetFactory.CreateAnnual(BudgetType.Entertainment_Movies, "SIFF", 600.00, 12);

			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Christmas", 1600.00, 11);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Michelle", 150, 3);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Chris", 150, 1);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Thea", 150, 10);
			BudgetFactory.CreateAnnual(BudgetType.Gift_FamilyFriends, "Dan", 100, 8);

			var budget = BudgetFactory.GetBudget();
			budget.Title = title;
			return budget;
		}

		public static Report DoMonthlyReport()
		{
			var month = GetMonth();
			var year = GetYear();
			var report = new Report(month, year);

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

		private static int GetYear()
		{
			int year;
			Console.Write($"Enter year{_prompt}");
			do
			{
				var input = Console.ReadLine();
				if (int.TryParse(input, out year) && year > 2000 && year <= DateTime.Now.Year)
					break;
				Console.Write($"Invalid Year. Try again{_prompt}");
			} while (true);
			return year;
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
