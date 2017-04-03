using System;
using System.Collections.Generic;

namespace Retire
{

	public abstract class BudgetEntry
	{
		public string Label { get; private set; }
		public BudgetCategory Category { get; private set; }
		private double _amount;

		public BudgetEntry(double amount, string label, BudgetType budgetType)
		{
			_amount = amount;
			this.Label = label;
			this.Category = BudgetCategoryFactory.GetBudgetCategory(budgetType);
		}

		public double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _amount : 0.0;
		}

		protected virtual bool ValidMonth(int month)
		{
			return month > 0 && month < 13;
		}


		public virtual string Serialize()
		{
			var categoryString = BudgetCategoryFactory.Serialize(Category.BudgetType);
			var amountString = _amount.ToString();
			return $"{categoryString},{amountString},{Label}";
		}

}

	public class BudgetEntryMonthly : BudgetEntry
	{
		public BudgetEntryMonthly(double amount, string label, BudgetType budgetType)
			: base(amount, label, budgetType)
		{
		}

		public override string Serialize()
		{
			return "Monthly," + base.Serialize();
		}
	}

	public class BudgetEntryBiMonthly : BudgetEntry
	{
		public bool Odd { get; private set; }

		public BudgetEntryBiMonthly(double amount, int month, string label, BudgetType budgetType)
			: base(amount, label, budgetType)
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

		public override string Serialize()
		{
			var month = Odd ? "1" : "2";
			return $"BiMonthly,{month}," + base.Serialize();
		}
	}

	public class BudgetEntryAnnual : BudgetEntry
	{
		private int _month;

		public BudgetEntryAnnual(double amount, int month, string label, BudgetType budgetType)
			: base(amount, label, budgetType)
		{
			_month = month;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && month == _month;
		}

		public override string Serialize()
		{
			var month = _month.ToString();
			return $"Annual,{month}," + base.Serialize();
		}
	}

	public class BudgetEntryBiAnnual : BudgetEntry
	{
		private int _month;

		public BudgetEntryBiAnnual(double amount, int month, string label, BudgetType budgetType)
			: base(amount, label, budgetType)
		{
			if (month > 6)
				month -= 6;
			_month = month;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && (month == _month || month == _month + 6);
		}

		public override string Serialize()
		{
			var month = _month.ToString();
			return $"BiAnnual,{month}," + base.Serialize();
		}
	}
}
