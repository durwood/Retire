using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Retire
{
	public abstract class BudgetEntryDto
	{
		protected double Amount;
		public string Label;
		public BudgetCategory Category;

		public BudgetEntryDto(BudgetEntry budgetEntry)
		{
			Amount = budgetEntry.Amount;
			Label = budgetEntry.Label;
			Category = budgetEntry.Category;
		}



		// Create Dto from binary object
		// Create Object from string
	}

	public class BudgetEntryMonthlyDto : BudgetEntryDto
	{
		public BudgetEntryMonthlyDto(BudgetEntryMonthly budgetEntry) : base(budgetEntry)
		{
		}
	}

	public abstract class BudgetEntry
	{
		public string Label { get; private set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public BudgetType BudgetType;
		public double Amount { get; private set; }

		[JsonIgnore]
		public BudgetCategory Category { get; private set; }


		public BudgetEntry(double amount, string label, BudgetType budgetType)
		{
			Amount = amount;
			this.Label = label;
			BudgetType = budgetType;
			this.Category = BudgetCategoryFactory.GetBudgetCategory(budgetType);
		}

		public virtual double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? Amount : 0.0;
		}

		protected virtual bool ValidMonth(int month)
		{
			return month > 0 && month < 13;
		}


		public virtual string Serialize()
		{
			var categoryString = BudgetCategoryFactory.Serialize(Category.BudgetType);
			var amountString = Amount.ToString();
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
		[JsonProperty]
		private bool Odd { get; set; }

		public BudgetEntryBiMonthly(double amount, int month, string label, BudgetType budgetType)
			: base(amount, label, budgetType)
		{
			this.Odd = IsOdd(month);
		}

		[JsonConstructor]
		public BudgetEntryBiMonthly(double amount, bool odd, string label, BudgetType budgetType)
	: base(amount, label, budgetType)
		{
			this.Odd = odd;
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
		[JsonProperty]
		private int Month { get; set; }

		public BudgetEntryAnnual(double amount, int month, string label, BudgetType budgetType)
			: base(amount, label, budgetType)
		{
			Month = month;
		}

		protected override bool ValidMonth(int month)
		{
			return base.ValidMonth(month) && month == Month;
		}

		public override string Serialize()
		{
			var month = Month.ToString();
			return $"Annual,{month}," + base.Serialize();
		}
	}

	public class BudgetEntryBiAnnual : BudgetEntry
	{
		[JsonProperty]
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

    class BudgetEntryDaily : BudgetEntry
    {
		private Dictionary<int, double> _monthly = new Dictionary<int, double> {
			{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12, 0}};
        
		[JsonProperty]
        private string Start;

        public BudgetEntryDaily(double amount, string label, BudgetType budgetType, string start) : base(amount, label, budgetType)
        {
            Start = start;

			var startDay = DateTime.Parse(Start);
			var lastDate = DateTime.Parse($"Dec 31, {startDay.Year}");  // TODO: Should this really be based of year passed in? Shouldn't the year be stored in the budget?
			const int dayPeriod = 1;
			for (DateTime day = startDay; day <= lastDate; day = day.AddDays(dayPeriod))
				_monthly[day.Month] += Amount;
		}

		public override double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _monthly[month] : 0.0;
		}
    }

	class BudgetEntryWeekly : BudgetEntry
	{
		private Dictionary<int, double> _monthly = new Dictionary<int, double> {
			{ 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12, 0}};

		[JsonProperty]
		private int Period;

		[JsonProperty]
		private string Start;

		[JsonProperty]
		private int Max;

		[JsonConstructor]
		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType, int period, string start, int max) : base(amount, label, budgetType)
		{
			Period = period;
			Start = start;
			Max = max;

			var startDay = DateTime.Parse(Start);
            var lastDate = DateTime.Parse($"Dec 31, {startDay.Year}"); // TODO: Should this really be based of year passed in? Shouldn't the year be stored in the budget?
			var dayPeriod = Period * 7;
			var count = 0;
			for (DateTime day = startDay; day <= lastDate && count++ < Max; day = day.AddDays(dayPeriod))
				_monthly[day.Month] += Amount;                 
		}

		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType, int period, string start) : this(amount, label, budgetType, period, start, 52)
		{

		}

		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType, int period) : this(amount, label, budgetType, period, "Jan 1")
		{
			
		}

		public BudgetEntryWeekly(double amount, string label, BudgetType budgetType) : this(amount, label, budgetType, 1, "Jan 1")
		{
		}

		public override double GetMonthEntry(int month)
		{
			return ValidMonth(month) ? _monthly[month] : 0.0;
		}

		public override string Serialize()
		{
			return $"Weekly,{Start},{Period},{Max}" + base.Serialize();
		}
	}
}
