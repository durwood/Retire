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
	}
	
}
