using System;

namespace Retire
{

	public class BudgetEntry
	{
		public double AnnualizedAmount { get; private set; }
		public string Label { get; private set; }
		public string Category { get; private set; }
		public string SubCategory { get; private set; }

		public BudgetEntry(double amount, string label, string category, string subCategory)
		{
			this.AnnualizedAmount = amount;
			this.Label = label;
			this.Category = category;
			this.SubCategory = subCategory;
		}
	}

	public class BudgetEntryMonthly : BudgetEntry
	{
		public double Amount { get; private set; }

		public BudgetEntryMonthly(double amount, string label, string category, string subCategory) : base(amount * 12, label, category, subCategory)
		{
			this.Amount = amount;
		}
	}
}
