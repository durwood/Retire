using System;
using System.Collections.Generic;

namespace Retire
{

	public abstract class BudgetEntry
	{
		public string Label { get; private set; }
		public string Category { get; private set; }
		public string SubCategory { get; private set; }
		private double _amount;

		public BudgetEntry(double amount, string label, string category, string subCategory)
		{
			_amount = amount;
			this.Label = label;
			this.Category = category;
			this.SubCategory = subCategory;
		}

		public double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _amount : 0.0;
		}

		protected virtual bool ValidMonth(int month)
		{
			return month > 0 && month < 13;
		}
	}

	public class BudgetEntryMonthly : BudgetEntry
	{
		public BudgetEntryMonthly(double amount, string label, string category, string subCategory)
			: base(amount, label, category, subCategory)
		{
		}
	}

	public class BudgetEntryBiMonthly : BudgetEntry
	{
		public bool Odd { get; private set; }

		public BudgetEntryBiMonthly(double amount, int month, string label, string category, string subCategory)
			: base(amount, label, category, subCategory)
		{
			this.Odd = IsOdd(month);
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
		private int _month;

		public BudgetEntrySingleMonth(double amount, int month, string label, string category, string subCategory)
			: base(amount, label, category, subCategory)
		{
			_month = month;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && month == _month;
		}
	}

	public class BudgetEntryBiAnnual : BudgetEntry
	{
		private int _month;

		public BudgetEntryBiAnnual(double amount, int month, string label, string category, string subCategory)
			: base(amount, label, category, subCategory)
		{
			if (month > 6)
				month -= 6;
			_month = month;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && (month == _month || month == _month + 6);
		}
	}
}
