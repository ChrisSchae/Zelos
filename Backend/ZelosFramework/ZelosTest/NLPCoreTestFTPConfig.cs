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
	public class NLPCoreTestFTPConfig
	{
		public Script DocWithGivenUserAndFTP { get; private set; }
		public Script DocWithAnonymousUserAndFTP { get; private set; }

		[SetUp]
		public void SetUp()
		{
			INlpAnalyzer analyzer = new CatalystAnalyser();

			this.DocWithAnonymousUserAndFTP = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip'"); 
			this.DocWithGivenUserAndFTP = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with the user 'ABC' and password 'pw123' from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip");
		}

		[Test]
		public void AsureAnalysisDoesAnythingAnonymous()
		{
			Assert.IsNotNull(DocWithAnonymousUserAndFTP);

		}

		

		[Test]
		public void AsureTagCountIsCorrectAnonymous()
		{
			Assert.AreEqual(35, DocWithAnonymousUserAndFTP.TokenizedDoc.Count);

		}

		[Test]
		public void AsureFTPServerIsRealisedAnonymous()
		{
			Assert.IsTrue(DocWithAnonymousUserAndFTP.IsFtpServerScript);
			Assert.AreEqual("opendata.dwd.de", DocWithAnonymousUserAndFTP.FtpSource.Url);
			Assert.IsNull(DocWithAnonymousUserAndFTP.FtpSource.User);
			Assert.IsNull(DocWithAnonymousUserAndFTP.FtpSource.Password);
			Assert.IsNotNull(DocWithAnonymousUserAndFTP.FtpSource.FilePath);
			Assert.AreEqual("/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip", DocWithAnonymousUserAndFTP.FtpSource.FilePath);
		}

		[Test]
		public void GetsFileFromFTPAsync()
		{
			var executionResult = this.DocWithAnonymousUserAndFTP.ExecuteScript();

			Assert.IsInstanceOf(typeof(FileInfo), executionResult);
		}

		[Test]
		public void AsureAnalysisDoesAnything()
		{
			Assert.IsNotNull(DocWithGivenUserAndFTP);

		}

		[Test]
		public void AsureFTPServerIsRealised()
		{
			Assert.IsTrue(DocWithGivenUserAndFTP.IsFtpServerScript);
			Assert.AreEqual("opendata.dwd.de", DocWithGivenUserAndFTP.FtpSource.Url);
			Assert.IsNotNull(DocWithGivenUserAndFTP.FtpSource.User);
			Assert.IsNotNull(DocWithGivenUserAndFTP.FtpSource.Password);
			Assert.IsNotNull(DocWithGivenUserAndFTP.FtpSource.FilePath);
			Assert.AreEqual("ABC", DocWithGivenUserAndFTP.FtpSource.User);
			Assert.AreEqual("pw123", DocWithGivenUserAndFTP.FtpSource.Password);
			Assert.AreEqual("/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip", DocWithGivenUserAndFTP.FtpSource.FilePath);
		}
	}
}