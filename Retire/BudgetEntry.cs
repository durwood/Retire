using System;
using System.Collections.Generic;

namespace Retire
{

	public abstract class BudgetEntry
	{
		public string Label { get; private set; }
		public string Category { get; private set; }
		public string SubCategory { get; private set; }

		public BudgetEntry(string label, string category, string subCategory)
		{
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
			: base(label, category, subCategory)
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
			: base(label, category, subCategory)
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

	public class BudgetEntrySingleMonth : BudgetEntry
	{
		private double _amount;
		private int _month;

		public BudgetEntrySingleMonth(double amount, int month, string label, string category, string subCategory)
			: base(label, category, subCategory)
		{
			_amount = amount;
			_month = month;
		}


		public override double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _amount : 0.0;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && month == _month;
		}
	}

	public class BudgetEntryBiAnnual : BudgetEntry
	{
		private double _amount;
		private int _month;

		public BudgetEntryBiAnnual(double amount, int month, string label, string category, string subCategory)
			: base(label, category, subCategory)
		{
			if (month > 6)
				month -= 6;
			_month = month;
			_amount = amount;
		}

		public override double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _amount : 0.0;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && (month == _month || month == _month + 6);
		}
	}
}
