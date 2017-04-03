using System;
using NUnit.Framework;

namespace Retire
{

	[TestFixture]
	public class ReportGenerationTests
	{
		private Report _report;

		[SetUp]
		public void SetUp()
		{
			_report = new Report(1);
		}

		[Test]
		public void CanCreateEmptyReport()
		{
			Assert.That(_report.GetReport().Count, Is.EqualTo(0));
		}

		[Test]
		public void CanCreateSimpleReport()
		{
			_report.AddExpenditure(BudgetType.Auto, 80.00);
			var report = _report.GetReport();
			Assert.That(report.Count, Is.EqualTo(1));
			Assert.That(report[BudgetType.Auto.ToString()], Is.EqualTo(80.00));
		}

		[Test]
		public void SubCategoriesAreStoredInParent()
		{
			_report.AddExpenditure(BudgetType.Auto_Gas, 80.00);
			var report = _report.GetReport();
			Assert.That(report.Count, Is.EqualTo(1));
			Assert.That(report[BudgetType.Auto.ToString()], Is.EqualTo(80.00));
			Assert.That(report.ContainsKey(BudgetType.Auto_Gas.ToString()), Is.False);
		}

		[Test]
		public void SubCategoriesAccumulateInParent()
		{
			_report.AddExpenditure(BudgetType.Auto_Gas, 80.00);
			_report.AddExpenditure(BudgetType.Auto_Gas, 20.00);
			_report.AddExpenditure(BudgetType.Auto, 100.00);

			var report = _report.GetReport();
			Assert.That(report.Count, Is.EqualTo(1));
			Assert.That(report[BudgetType.Auto.ToString()], Is.EqualTo(200.00));
		}
	}

}
