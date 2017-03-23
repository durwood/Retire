using System.Collections.Generic;
using System;
            
namespace Retire
{
	public enum BudgetType
	{
		Auto_Gas,
		Auto_Insurance,
		Digital_Movies,
		Digital_Music,
		Digital_Subscription,
		Entertainment_SportingEvents,
		Home_Mortgage,
		Medical_Insurance,
		Shopping_Subscription,
		Utilities_Electricity,
		Utilities_Gas, 
		Utilities_InternetCable,
		Utilities_WaterSewerWaste,
		Gift_FamilyFriends,
		Entertainment_Movies
	}

	public class BudgetCategory
	{
		public BudgetType BudgetType;
		public string MainCategory { get; private set; }
		public string SubCategory { get; private set; }

		public BudgetCategory(BudgetType budgetType)
		{
			BudgetType = budgetType;
			var mainAndSub = budgetType.ToString().Split('_');
			MainCategory = mainAndSub[0];
			SubCategory = mainAndSub[1];
		}

		public override string ToString()
		{
			return string.Format($"{MainCategory}:{SubCategory}");
		}
	}

	public static class BudgetCategoryFactory
	{
		private static Dictionary<BudgetType, BudgetCategory> _budgetCategory = new Dictionary<BudgetType, BudgetCategory>();

		public static BudgetCategory GetBudgetCategory(BudgetType budgetType)
		{
			return _budgetCategory[budgetType];
		}

		static BudgetCategoryFactory()
		{
			foreach (BudgetType budgetType in Enum.GetValues(typeof(BudgetType)))
			{
				_budgetCategory.Add(budgetType, new BudgetCategory(budgetType));
			}
		}
	}
}