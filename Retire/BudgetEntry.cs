using System;
using System.Collections.Generic;

namespace Retire
{

	public abstract class BudgetEntry
	{
		public string Label { get; private set; }
		public BudgetCategory Category { get; private set; }
		protected double _amount;

		public BudgetEntry(double amount, string label, BudgetType budgetType)
		{
			_amount = amount;
			this.Label = label;
			this.Category = BudgetCategoryFactory.GetBudgetCategory(budgetType);
		}

		public virtual double GetMonthEntry(int month)
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

	class BudgetEntryWeekly : BudgetEntry
	{
		private Dictionary<int, double> _monthly = new Dictionary<int, double> {
			{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12, 0}};
		int _period;
		int _start;

		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType, int period, int start) : base(amount, label, budgetType)
		{
			_period = period;
			_start = start;

			var date = new DateTime(2017, 1, 1);
			for (int week = _start; week <= 52; week += _period)
			{
				var curDate = date.AddDays(week * 7);
				var month = curDate.Month;
				_monthly[month] += _amount;                 
			}
		}

		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType, int period) : this(amount, label, budgetType, period, 1)
		{
			
		}

		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType) : this(amount, label, budgetType, 1, 1)
		{
		}

		public override double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _monthly[month] : 0.0;
		}

		public override string Serialize()
		{
			return $"Weekly,{_start},{_period}," + base.Serialize();
		}
	}
}
