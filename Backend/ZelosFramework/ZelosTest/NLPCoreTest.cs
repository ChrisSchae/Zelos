using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZelosFramework;
using ZelosFramework.FileHandling;
using ZelosFramework.NLP_Core;
using ZelosFramework.Scheduling;
using NUnit.Framework;

namespace ZelosTest
{
	public class NLPCoreTest
	{
		public Script Doc1 { get; private set; }
		public Script Doc2 { get; private set; }
		public Script DocWithTime { get; private set; }

		[SetUp]
		public void SetUp()
		{
			INlpAnalyzer analyzer = new CatalystAnalyser();

			this.Doc1 = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip'");
			this.Doc2 = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with the user 'ABC' and password 'pw123' from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip");
			this.DocWithTime = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip' every week at Tuesday at 3 PM");
		}

		[Test]
		public void AsureAnalysisDoesAnything_Doc1()
		{
			Assert.IsNotNull(Doc1);

		}

		[Test]
		public void AsureAnalysisDoesAnything_Doc2()
		{
			Assert.IsNotNull(Doc2);

		}

		[Test]
		public void AsureTagCountIsCorrect_Doc1()
		{
			Assert.AreEqual(35, Doc1.TokenizedDoc.Count);

		}

		[Test]
		public void AsureFTPServerIsRealised_Doc1()
		{
			Assert.IsTrue(Doc1.IsFtpServerScript);
			Assert.AreEqual("opendata.dwd.de", Doc1.FtpSource.Url);
			Assert.IsNull(Doc1.FtpSource.User);
			Assert.IsNull(Doc1.FtpSource.Password);
			Assert.IsNotNull(Doc1.FtpSource.FilePath);
			Assert.AreEqual("/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip", Doc1.FtpSource.FilePath);
		}

		[Test]
		public void AsureFTPServerIsRealised_Doc2()
		{
			Assert.IsTrue(Doc2.IsFtpServerScript);
			Assert.AreEqual("opendata.dwd.de", Doc2.FtpSource.Url);
			Assert.IsNotNull(Doc2.FtpSource.User);
			Assert.IsNotNull(Doc2.FtpSource.Password);
			Assert.IsNotNull(Doc2.FtpSource.FilePath);
			Assert.AreEqual("ABC", Doc2.FtpSource.User);
			Assert.AreEqual("pw123", Doc2.FtpSource.Password);
			Assert.AreEqual("/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip", Doc2.FtpSource.FilePath);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrect_Doc1()
		{
			Assert.AreEqual(FileType.ZIP, Doc1.SourceFileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrect_Doc2()
		{
			Assert.AreEqual(FileType.ZIP, Doc2.SourceFileType);
		}

		[Test]
		public void AsureRecognizedStartTimeIsCorrect()
		{
			Assert.NotNull(DocWithTime.SchedulingConfig);
			Assert.AreEqual(IntervalType.Week, DocWithTime.SchedulingConfig.IntervalType);
			Assert.AreEqual(15, DocWithTime.SchedulingConfig.Hour);
			Assert.AreEqual(0, DocWithTime.SchedulingConfig.Minute);
			Assert.AreEqual(2, DocWithTime.SchedulingConfig.DayOffset);
		}

		[Test]
		public void GetsFileFromFTPAsync()
		{
			var executionResult = this.Doc1.ExecuteScript();

			Assert.IsInstanceOf(typeof(FileInfo), executionResult);
		}
	}
}