using NUnit.Framework;
using ZelosFramework.NLP_Core;
using ZelosFramework.Scheduling;

namespace ZelosTest
{
    class NPLCoreTestWithTime
    {
        public Script DocWithTimeWeekly { get; private set; }
		public Script DocWithTimeMonthly { get; private set; }
		public Script DocWithTimeDaily { get; private set; }

		[SetUp]
		public void SetUp()
		{
			INlpAnalyzer analyzer = new CatalystAnalyser();

			this.DocWithTimeWeekly = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip' every week at Tuesday at 3 PM");
			this.DocWithTimeMonthly = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip' every month at the twenty-third day at 2");
			this.DocWithTimeDaily = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip' every day at 15:15");
		}

		[Test]
		public void AsureRecognizedStartTimeIsCorrect_Weekly()
		{
			Assert.NotNull(DocWithTimeWeekly.SchedulingConfig);
			Assert.AreEqual(IntervalType.Week, DocWithTimeWeekly.SchedulingConfig.IntervalType);
			Assert.AreEqual(15, DocWithTimeWeekly.SchedulingConfig.Hour);
			Assert.AreEqual(0, DocWithTimeWeekly.SchedulingConfig.Minute);
			Assert.AreEqual(2, DocWithTimeWeekly.SchedulingConfig.DayOffset);
		}

		[Test]
		public void AsureRecognizedStartTimeIsCorrect_Monthly()
		{
			Assert.NotNull(DocWithTimeMonthly.SchedulingConfig);
			Assert.AreEqual(IntervalType.Month, DocWithTimeMonthly.SchedulingConfig.IntervalType);
			Assert.AreEqual(2, DocWithTimeMonthly.SchedulingConfig.Hour);
			Assert.AreEqual(0, DocWithTimeMonthly.SchedulingConfig.Minute);
			Assert.AreEqual(23, DocWithTimeMonthly.SchedulingConfig.DayOffset);
		}

		[Test]
		public void AsureRecognizedStartTimeIsCorrect_Daily()
		{
			Assert.NotNull(DocWithTimeDaily.SchedulingConfig);
			Assert.AreEqual(IntervalType.Day, DocWithTimeDaily.SchedulingConfig.IntervalType);
			Assert.AreEqual(15, DocWithTimeDaily.SchedulingConfig.Hour);
			Assert.AreEqual(15, DocWithTimeDaily.SchedulingConfig.Minute);
			Assert.AreEqual(-1, DocWithTimeDaily.SchedulingConfig.DayOffset);
		}
	}
}
