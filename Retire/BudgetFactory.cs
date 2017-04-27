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

        internal static void CreateDaily(BudgetType budgetType, string label, double amount, string start="Jan 1")
        {
            _budget.AddEntry(new BudgetEntryDaily(amount, label, budgetType, start));
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

        private static void GetNextValueAndAdvance(ref string[] inputs, out string output)
        {
            output = inputs[0];
            inputs = inputs.Skip(1).ToArray();
        }

        private static void GetNextValueAndAdvance(ref string[] inputs, out int output)
		{
            output = int.Parse(inputs[0]);
			inputs = inputs.Skip(1).ToArray();
		}

		static void GetCommonParameters(ref string[] components, out double amount, out string label, out BudgetType budgetType)
		{
			budgetType = BudgetCategoryFactory.DeSerialize(components[0]);
			amount = double.Parse(components[1]);
			label = components[2];
			components = components.Skip(3).ToArray();
		}

		public static BudgetEntry CreateBudgetEntry(params string[] components)
		{
			BudgetEntry result = null;

			double amount;
			string label;
			BudgetType budgetType;
			int month;
            string dayStart;
			int weekPeriod;
            int weekMax;

            string type;
            GetNextValueAndAdvance(ref components, out type);
			GetCommonParameters(ref components, out amount, out label, out budgetType);
			switch (type)
			{


				case "Annual":
                    GetNextValueAndAdvance(ref components, out month);
					result = new BudgetEntryAnnual(amount, month, label, budgetType);
					break;
				case "BiAnnual":
					GetNextValueAndAdvance(ref components, out month);
					result = new BudgetEntryBiAnnual(amount, month, label, budgetType);
					break;
				case "Monthly":
					result = new BudgetEntryMonthly(amount, label, budgetType);
					break;
				case "BiMonthly":
					GetNextValueAndAdvance(ref components, out month);
					result = new BudgetEntryBiMonthly(amount, month, label, budgetType);
					break;
				case "Weekly":
					GetNextValueAndAdvance(ref components, out dayStart);
					GetNextValueAndAdvance(ref components, out weekPeriod);
                    GetNextValueAndAdvance(ref components, out weekMax);
					result = new BudgetEntryWeekly(amount, label, budgetType, weekPeriod, dayStart, weekMax);
					break;
				case "Daily":
					GetNextValueAndAdvance(ref components, out dayStart);
					result = new BudgetEntryDaily(amount, label, budgetType, dayStart);
					break;
				default:
					throw new ArgumentException($"Invalid Budget Entry Type {type}");
			}
			return result;
		}
}
}