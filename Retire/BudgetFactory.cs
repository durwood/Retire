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

		internal static void CreateWeekly(BudgetType budgetType, string label, double amount, int period=1, string start="Jan 4", int max=52)
		{
			_budget.AddEntry(new BudgetEntryWeekly(amount, label, budgetType, period, start, max));
		}

        public static void CreateBudget(int year, string user="")
        {
            _budget = new Budget(year, user);
        }

		public static Budget GetBudget()
		{
			return _budget;
		}

		static BudgetFactory()
		{
		}

        private static string[] GetValueAndAdvance(string[] inputs, out string output)
        {
            output = inputs[0];
            return inputs.Skip(1).ToArray();
        }

		private static string[] GetValueAndAdvance(string[] inputs, out int output)
		{
            output = int.Parse(inputs[0]);
			return inputs.Skip(1).ToArray();
		}

		public static BudgetEntry CreateBudgetEntry(string[] components)
		{
			BudgetEntry result = null;
			double amount;
			string label;
			BudgetType budgetType;
			int month;
            string dayStart;
			int weekPeriod;
            string type;
            components = GetValueAndAdvance(components, out type);

			switch (type)
			{

				case "Annual":
                    components = GetValueAndAdvance(components, out month);
					GetCommonParameters(components, out amount, out label, out budgetType);
					result = new BudgetEntryAnnual(amount, month, label, budgetType);
					break;
				case "BiAnnual":
					components = GetValueAndAdvance(components, out month);
					GetCommonParameters(components, out amount, out label, out budgetType);
					result = new BudgetEntryBiAnnual(amount, month, label, budgetType);
					break;
				case "Monthly":
					GetCommonParameters(components, out amount, out label, out budgetType);
					result = new BudgetEntryMonthly(amount, label, budgetType);
					break;
				case "BiMonthly":
					components = GetValueAndAdvance(components, out month);
					GetCommonParameters(components, out amount, out label, out budgetType);
					result = new BudgetEntryBiMonthly(amount, month, label, budgetType);
					break;
				case "Weekly":
					components = GetValueAndAdvance(components, out dayStart);
					components = GetValueAndAdvance(components, out weekPeriod);
					GetCommonParameters(components, out amount, out label, out budgetType);
					result = new BudgetEntryWeekly(amount, label, budgetType, weekPeriod, dayStart);
					break;
				case "daily":
					components = GetValueAndAdvance(components, out dayStart);
					GetCommonParameters(components, out amount, out label, out budgetType);
					result = new BudgetEntryDaily(amount, label, budgetType, dayStart);
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