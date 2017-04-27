using System;
using Newtonsoft.Json;
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
			_report = new Report(1, 2017);
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

        [Test]
        public void CanGetExpenseTotal()
        {
            _report.AddExpenditure(BudgetType.Income, 1000);
            _report.AddExpenditure(BudgetType.Auto, 200);
            Assert.That(_report.Expenses, Is.EqualTo(200));
        }

		[Test]
		public void CanAccumulateDetailedIncomeCategories()
		{
			_report.AddExpenditure(BudgetType.Income, 100);
			_report.AddExpenditure(BudgetType.Income_Airbnb, 200);

			var report = _report.GetReport();
            Console.WriteLine(JsonConvert.SerializeObject(report));

			Assert.That(report.Count, Is.EqualTo(2));
			Assert.That(report[BudgetType.Income.ToString()], Is.EqualTo(100.00));
			Assert.That(report[BudgetType.Income_Airbnb.ToString()], Is.EqualTo(200.00));

			report = _report.GetReport(incomeDetails: false);
			Assert.That(report.Count, Is.EqualTo(1));
			Assert.That(report[BudgetType.Income.ToString()], Is.EqualTo(300.00));
		}

		[Test]
		public void CanSerializeReport()
		{
			_report.AddExpenditure(BudgetType.Auto_Gas, 80.00);
			_report.AddExpenditure(BudgetType.Auto, 20.00);
			_report.AddExpenditure(BudgetType.Income_Salary, 300.00);
			var savedReport = _report.Serialize();
			Console.WriteLine(savedReport);
			Report report = Report.DeSerialize(savedReport);
			Assert.That(report.Month, Is.EqualTo(_report.Month));
			var entries = report.GetReport();
			Assert.That(entries["Auto"], Is.EqualTo(100.00));
			Assert.That(entries["Income_Salary"], Is.EqualTo(300.00));
		}

		[Test]
		public void CanJsonSerializeReport()
		{
			_report.AddExpenditure(BudgetType.Auto_Gas, 80.00);
			_report.AddExpenditure(BudgetType.Auto, 20.00);
			_report.AddExpenditure(BudgetType.Income_Salary, 300.00);
			var savedReport = JsonConvert.SerializeObject(_report);
			Console.WriteLine(savedReport);

			var report = JsonConvert.DeserializeObject<Report>(savedReport);
			Console.WriteLine(JsonConvert.SerializeObject(report));

		}

        [Test]
        public void ReportAccumulatesIncomeAsAppropriateBasedOnWhetherDetailsArePreserved()
        {
			_report.AddExpenditure(BudgetType.Income, 100.00);
			_report.AddExpenditure(BudgetType.Income_Salary, 300.00);

            var report = _report.GetReport();
            Assert.That(report.Count, Is.EqualTo(2));
            Assert.That(report["Income"], Is.EqualTo(100.0));
            Assert.That(report["Income_Salary"], Is.EqualTo(300.0));

            report = _report.GetReport(incomeDetails: false);
			Assert.That(report.Count, Is.EqualTo(1));
			Assert.That(report["Income"], Is.EqualTo(400.0));
        }

        private static void PrintReport(Report report)
        {
            Console.WriteLine(JsonConvert.SerializeObject(report));
        }
	}

}
