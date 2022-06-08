using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZelosFramework.NLP_Core;
using ZelosFramework.NLP_Core.Catalyst;
using ZelosFramework.NLP_Core.OpenNLP;
using ZelosFramework.NLP_Core.Cherub;

namespace ZelosTest
{
    class NLPCoreTestGeneralAndLibComparison
	{
		public Script DocWithOpenNLP { get; private set; }
		public Script DocWithCatalyst { get; private set; }
		public Script DocWithCherub { get; private set; }

		[SetUp]
		public void SetUp()
		{
			INlpAnalyzer analyzer = new CatalystAnalyser();
			this.DocWithCatalyst = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip'");

			// analyzer = new OpenNLPAnalyser();
			// this.DocWithOpenNLP = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip'");

			analyzer = new CherubAnalyser();
			this.DocWithCherub = analyzer.Analyse("I want to download a ZIP file from the FTP server 'opendata.dwd.de' with an anonymous user from path '/climate_environment/CDC/observations_germany/climate/subdaily/air_temperature/recent/terminwerte_TU_00071_akt.zip'");

		}

		[Test]
		public void AsureAnalysisDoesAnythingCatalyst()
		{
			Assert.IsNotNull(DocWithCatalyst);

		}

		[Test]
		public void AsureAnalysisDoesAnythingCherub()
		{
			Assert.IsNotNull(DocWithCherub);

		}


		[Test]
		public void AsureAnalysisDoesAnythingOpenNLP()
		{
			Assert.IsNotNull(DocWithOpenNLP);

		}


		[Test]
		public void AsureTagCountIsCorrectCatalyst()
		{
			Assert.AreEqual(35, DocWithCatalyst.TokenizedDoc.Count);

		}

		[Test]
		public void AsureTagCountIsCorrectCherub()
		{
			Assert.AreEqual(35, DocWithCherub.TokenizedDoc.Count);

		}

		[Test]
		public void AsureTagCountIsCorrectOpenNLP()
		{
			Assert.AreEqual(35, DocWithOpenNLP.TokenizedDoc.Count);

		}
	}
}
