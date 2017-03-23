using System;
using System.Globalization;

namespace Retire
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var budgetTitle = "2017 Budget";

			Budget budget = new Budget(budgetTitle);

			budget.AddEntry(new BudgetEntryMonthly( 2000.00, "Mortgate", "House", "Mortgage"));
			budget.AddEntry(new BudgetEntryMonthly( 242.00, "Comcast", "Utilities", "InternetCable"));
			budget.AddEntry(new BudgetEntryMonthly(  60.00, "Gas", "Auto", "Gas"));
			budget.AddEntry(new BudgetEntryMonthly(  90.00, "PSE", "Utilities", "NaturalGas"));
			budget.AddEntry(new BudgetEntryBiMonthly(190.00, 1, "SPU", "Utilities", "WaterSewerWaste"));
			budget.AddEntry(new BudgetEntryBiMonthly(80.00, 1, "SCL", "Utilities", "Electricity"));
			budget.AddEntry(new BudgetEntrySpecificMonths(900.00, 1, "Mariners", "Entertainment", "SportingEvents"));
			budget.AddEntry(new BudgetEntrySpecificMonths("BMW Insurance", "Auto", "Insurance", 337.00, 4, 10));
			budget.AddEntry(new BudgetEntrySpecificMonths("Vespa Insurance", "Auto", "Insurance", 366.00, 4));

			Console.WriteLine(budget);
		}
	}

	
}
