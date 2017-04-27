using System.Collections.Generic;
using System;
            
namespace Retire
{
	public enum BudgetType
	{
		Auto,
		Auto_Gas,
		Auto_Insurance,
        Media,
		Media_Movies,
		Media_Music,
		Media_Subscription,
		Entertainment,
		Entertainment_Movies,
		Entertainment_SportingEvents,
		Gift,
        Gift_Donation,
		Gift_FamilyFriends,
        Gift_Tribute,
		Home,
		Home_Mortgage,
		Income,
		Income_Airbnb,
		Income_Unemployment,
		Income_Salary,
		Income_Misc,
		Medical,
		Medical_Insurance,
        Personal,
        Personal_Dining,
		Shopping,
		Shopping_Subscription,
		Utilities,
		Utilities_Electricity,
		Utilities_Gas, 
		Utilities_InternetCable,
		Utilities_WaterSewerWaste
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
			SubCategory = mainAndSub.Length > 1 ? mainAndSub[1] : "";
		}

        public static bool IsExpense(BudgetType type)
        {
            return new BudgetCategory(type).MainCategory != "Income";
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

		public static BudgetType GetBudgetType(string mainCategory, string subCategory)
		{
			foreach (var kvp in _budgetCategory)
			{
				if (kvp.Value.MainCategory.Equals(mainCategory) && kvp.Value.SubCategory.Equals(subCategory))
					return kvp.Key;
			}
			throw new ArgumentException($"Invalid Categories: {mainCategory}:{subCategory}");
		}

		public static ICollection<string> GetExpenseCategories()
		{
			var mainCategories = new HashSet<string>();
			foreach (var kvp in _budgetCategory)
			{
				var mainCategory = kvp.Value.MainCategory;
				if (mainCategory != "Income")
				   	mainCategories.Add(kvp.Value.MainCategory);
			}
			return mainCategories;
		}

		public static ICollection<string> GetIncomeCategories()
		{
			var incomeCategories = new HashSet<string>();
			foreach (var kvp in _budgetCategory)
			{
				var category = kvp.Value;
				if (category.MainCategory == "Income")
					incomeCategories.Add(kvp.Value.SubCategory);
			}
			return incomeCategories;
		}

		public static string Serialize(BudgetType budgetType)
		{
			var category = GetBudgetCategory(budgetType);
			return string.IsNullOrWhiteSpace(category.SubCategory) ? category.MainCategory : $"{category.MainCategory}:{category.SubCategory}";
		}

		public static BudgetType DeSerialize(string budgetTypeString)
		{
			var components = budgetTypeString.Split(':');
			return GetBudgetType(components[0], components.Length > 1 ? components[1] : "");
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