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
			budget.AddEntry(new BudgetEntrySingleMonth(900.00, 1, "Mariners", "Entertainment", "SportingEvents"));
			budget.AddEntry(new BudgetEntryBiAnnual(337.00, 4, "BMW Insurance", "Auto", "Insurance"));
			budget.AddEntry(new BudgetEntrySingleMonth(366.00, 4, "Vespa Insurance", "Auto", "Insurance"));

			Console.WriteLine(budget);
		}
	}

	
}
