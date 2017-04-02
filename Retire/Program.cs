using System;

namespace Retire
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var budget = CreateBudget("2017 Budget");
			Console.WriteLine(budget);

			Console.Write("Enter month for Report: ");
			var input = Console.ReadLine();

			int month;
			if (int.TryParse(input, out month))
			{
				Console.WriteLine($"Generating report for month {month}");
			}
		}

		public static Budget CreateBudget(string title)
		{
			BudgetFactory.CreateMonthly(BudgetType.Home_Mortgage, "Mortgage", 2000);
			BudgetFactory.CreateMonthly(BudgetType.Utilities_InternetCable, "Comcast", 242.00);
			BudgetFactory.CreateMonthly(BudgetType.Utilities_Gas, "Gas", 90.00);
			BudgetFactory.CreateBiMonthly(BudgetType.Utilities_WaterSewerWaste, "SPU", 190.00, 1);
			BudgetFactory.CreateBiMonthly(BudgetType.Utilities_Electricity, "SCL", 80.00, 1);

			BudgetFactory.CreateBiAnnual(BudgetType.Auto_Insurance, "BMW", 337.00, 4);
			BudgetFactory.CreateBiAnnual(BudgetType.Auto_Insurance, "BMW", 337.00, 10);
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

		public static void DoMonthlyReport(int month)
		{
		}
	}
}
