using System;

namespace Retire
{

	public abstract class BudgetEntry
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

		public abstract double GetMonthEntry(int month);

		protected virtual bool ValidMonth(int month)
		{
			return month > 0 && month < 13;
		}
	}

	public class BudgetEntryMonthly : BudgetEntry
	{
		public double Amount { get; private set; }

		public BudgetEntryMonthly(double amount, string label, string category, string subCategory)
			: base(amount * 12, label, category, subCategory)
		{
			this.Amount = amount;
		}

		public override double GetMonthEntry(int month)
		{
			return base.ValidMonth(month) ? Amount : 0.0;
		}
	}

	public class BudgetEntryBiMonthly : BudgetEntry
	{
		public double Amount { get; private set; }
		public bool Odd { get; private set; }

		public BudgetEntryBiMonthly(double amount, int month, string label, string category, string subCategory)
			: base(amount * 6, label, category, subCategory)
		{
			this.Amount = amount;
			this.Odd = IsOdd(month);
		}

		public override double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? Amount : 0.0;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && Odd == IsOdd(month);
		}

		private bool IsOdd(int month)
		{
			return month % 2 == 1;
		}
	}

	public class BudgetEntrySpecificMonths : BudgetEntry
	{
		public double Amount { get; private set; }
		public int Month { get; private set; }

		public BudgetEntrySpecificMonths(double amount, int month, string label, string category, string subCategory)
			: base(amount, label, category, subCategory)
		{
			this.Amount = amount;
			this.Month = month;
		}

		public override double GetMonthEntry(int month)
		{
			return month == Month ? Amount : 0.0;
		}
	}
}
