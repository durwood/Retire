using System;
using System.Collections.Generic;
using System.Linq;

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

		internal static void CreateWeekly(BudgetType budgetType, string label, double amount, int period=1, int start=1)
		{
			_budget.AddEntry(new BudgetEntryWeekly(amount, label, budgetType, period, start));
		}

		public static Budget GetBudget()
		{
			return _budget;
		}

		static BudgetFactory()
		{
			_budget = new Budget();
		}

		public static BudgetEntry CreateBudgetEntry(string[] components)
		{
			BudgetEntry result = null;
			double amount;
			string label;
			BudgetType budgetType;
			int month;
			int weekStart;
			int weekPeriod;
			switch (components[0])
			{
				case "Annual":
					month = int.Parse(components[1]);
					GetCommonParameters(components.Skip(2).ToArray(), out amount, out label, out budgetType);
					result = new BudgetEntryAnnual(amount, month, label, budgetType);
					break;
				case "BiAnnual":
					month = int.Parse(components[1]);
					GetCommonParameters(components.Skip(2).ToArray(), out amount, out label, out budgetType);
					result = new BudgetEntryBiAnnual(amount, month, label, budgetType);
					break;
				case "Monthly":
					GetCommonParameters(components.Skip(1).ToArray(), out amount, out label, out budgetType);
					result = new BudgetEntryMonthly(amount, label, budgetType);
					break;
				case "BiMonthly":
					month = int.Parse(components[1]);
					GetCommonParameters(components.Skip(2).ToArray(), out amount, out label, out budgetType);
					result = new BudgetEntryBiMonthly(amount, month, label, budgetType);
					break;
				case "Weekly":
					weekStart = int.Parse(components[1]);
					weekPeriod = int.Parse(components[2]);
					GetCommonParameters(components.Skip(3).ToArray(), out amount, out label, out budgetType);
					result = new BudgetEntryWeekly(amount, label, budgetType, weekPeriod, weekStart);
					break;
				default:
					throw new ArgumentException($"Invalid Budget Entry Type {components[0]}");
			}
			return result;
		}

		static void GetCommonParameters(string [] components, out double amount, out string label, out BudgetType budgetType)
		{
			budgetType = BudgetCategoryFactory.DeSerialize(components[0]);
			amount = double.Parse(components[1]);
			label = components[2];
		}

}
}