using System;
using NUnit.Framework;

namespace Retire
{
	[TestFixture]
	public class BudgetCategoryTests
	{
		[SetUp]
		public void SetUp()
		{
		}

		[Test]
		public void CanCreateSimpleBudgetCategory()
		{
			var category = new BudgetCategory(BudgetType.Utilities_Gas);
			Assert.That(category.MainCategory, Is.EqualTo("Utilities"));
			Assert.That(category.SubCategory, Is.EqualTo("Gas"));
		}

		[Test]
		public void CanSupportCategoryWithNoSubCategory()
		{
			var category = new BudgetCategory(BudgetType.Utilities);
			Assert.That(category.MainCategory, Is.EqualTo("Utilities"));
			Assert.That(category.SubCategory, Is.EqualTo(""));
		}

		[Test]
		public void ExpenseCategoriesContainParentCategories()
		{
			int numParents = 0;
			foreach (BudgetType budgetType in Enum.GetValues(typeof(BudgetType)))
			{
				var components = budgetType.ToString().Split('_');
				if (components.Length == 1 && components[0] != "Income")
					numParents += 1;
			}
			var mainCategories = BudgetCategoryFactory.GetExpenseCategories();
			Assert.That(mainCategories.Count, Is.EqualTo(numParents));
		}

		[Test]
		public void IncomeCategoriesContainSubCategoriesFromSpecialIncomeMainCategory()
		{
			int numCategories = 0;
			foreach (BudgetType budgetType in Enum.GetValues(typeof(BudgetType)))
			{
				var components = budgetType.ToString().Split('_');
				if (components.Length == 2 && components[0] == "Income")
					numCategories += 1;
			}
			var mainCategories = BudgetCategoryFactory.GetIncomeCategories();
			Assert.That(mainCategories.Count, Is.EqualTo(numCategories));
		}
	}
	
}
