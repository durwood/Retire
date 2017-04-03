using System;
using NUnit.Framework;

namespace Retire
{

	[TestFixture]
	public class ReportGenerationTests
	{
		[SetUp]
		public void SetUp()
		{
		}

		[Test]
		public void CanCreateEmptyReport()
		{
			var report = new Report(1);
			Assert.That(report.Entries.Count, Is.EqualTo(0));
		}

		[Test]
		public void CanCreateSimpleReport()
		{
			var report = new Report(1);
			report.AddExpenditure(BudgetType.Auto, 80.00);
			Assert.That(report.Entries.Count, Is.EqualTo(1));
		}
	}

}
