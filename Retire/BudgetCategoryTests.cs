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
				if (components[0] == "Income")
					numCategories += 1;
			}
			var mainCategories = BudgetCategoryFactory.GetIncomeCategories();
			Assert.That(mainCategories.Count, Is.EqualTo(numCategories));
		}

		[Test]
		public void CanGetBudgetTypeForValidCategories()
		{
			var budgetType = BudgetCategoryFactory.GetBudgetType("Income", "Misc");
			Assert.That(budgetType, Is.EqualTo(BudgetType.Income_Misc));
		}

		[Test]
		public void GetBudgetTypeHandlesEmptySubCategories()
		{
			var budgetType = BudgetCategoryFactory.GetBudgetType("Income", "");
			Assert.That(budgetType, Is.EqualTo(BudgetType.Income));
		}

		[Test]
		public void CanSerialize()
		{
			var budgetType = BudgetType.Income_Salary;
			var budgetTypeString = BudgetCategoryFactory.Serialize(budgetType);
			Assert.That(budgetTypeString, Is.EqualTo("Income:Salary"));
		}

		[Test]
		public void CanDeSerialize()
		{
			var budgetTypeString = "Income:Salary";
			var budgetType = BudgetCategoryFactory.DeSerialize(budgetTypeString);
			Assert.That(budgetType, Is.EqualTo(BudgetType.Income_Salary));
		}

		[Test]
		public void CanDeserializeMainCategoryOnly()
		{
			var budgetTypeString = "Income";
			var budgetType = BudgetCategoryFactory.DeSerialize(budgetTypeString);
			Assert.That(budgetType, Is.EqualTo(BudgetType.Income));
		}

		[Test]
		public void CanSerializeMainCategoryOnly()
		{
			var budgetType = BudgetType.Income;
			var budgetTypeString = BudgetCategoryFactory.Serialize(budgetType);
			Assert.That(budgetTypeString, Is.EqualTo("Income"));
		}

        [Test]
        public void IsIncomeWorks()
        {
            Assert.That(BudgetCategory.IsIncome(BudgetType.Income_Misc), Is.True);
            Assert.That(BudgetCategory.IsIncome(BudgetType.Auto_Gas), Is.False);
        }
	}
	
}
