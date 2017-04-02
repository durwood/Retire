using System;
using System.Collections.Generic;

namespace Retire
{

	class ReportEntry
	{
	}

	class Report
	{
		public int Month { get; private set; }
		private List<ReportEntry> _entries = new List<ReportEntry>();

		public Report(int month)
		{
			Month = month;
		}

		public List<ReportEntry> GetEntries()
		{
			return _entries;
		}

	}
}