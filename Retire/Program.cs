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
			budget.AddEntry(new BudgetEntryMonthly( 242.00, "Comcast", "Utilities", "Internet"));
			budget.AddEntry(new BudgetEntryMonthly(  60.00, "Gas", "Auto", "Gas"));

			Console.WriteLine(budget);
		}
	}

	
}
