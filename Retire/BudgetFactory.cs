using System;

namespace Retire
{
	static class BudgetFactory
	{
		private static Budget _budget;

		public static void CreateMonthly(BudgetType budgetType, string label, double amount)
		{
			_budget.AddEntry(new BudgetEntryMonthly(amount, label, budgetType));
		}

		public static void CreateBiMonthly(BudgetType budgetType, string label, double amount, int month)
		{
			_budget.AddEntry(new BudgetEntryBiMonthly(amount, month, label, budgetType));
		}

		public static void CreateBiAnnual(BudgetType budgetType, string label, double amount, int month)
		{
			_budget.AddEntry(new BudgetEntryBiAnnual(amount, month, label, budgetType));
		}

		public static void CreateAnnual(BudgetType budgetType, string label, double amount, int month)
		{
			_budget.AddEntry(new BudgetEntryAnnual(amount, month, label, budgetType));
		}

		public static Budget GetBudget()
		{
			return _budget;
		}

		static BudgetFactory()
		{
			_budget = new Budget();
		}

	}
}