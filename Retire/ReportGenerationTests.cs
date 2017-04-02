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
			var output = report.GetEntries();
			Assert.That(output.Count, Is.EqualTo(0));
		}
	}

}
