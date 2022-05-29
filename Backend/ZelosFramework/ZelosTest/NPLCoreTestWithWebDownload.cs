using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZelosFramework.NLP_Core;

namespace ZelosTest
{
    public class NPLCoreTestWithWebDownload
    {
		public Script DocWithGivenUserAndWebDownload { get; private set; }
		public Script DocWithAnonymousUserAndWebDownload { get; private set; }

		[SetUp]
		public void SetUp()
		{
			INlpAnalyzer analyzer = new CatalystAnalyser();

			this.DocWithAnonymousUserAndWebDownload = analyzer.Analyse("I want to download a CSV file from the website 'https://www.regelleistung.net/apps/datacenter/tendering-files/?productTypes=FCR,aFRR,mFRR&markets=CAPACITY,ENERGY&fileTypes=DEMANDS,RESULTS,ANONYMOUS_LIST_OF_BIDS&dateRange=2022-04,2022-04' by clicking on the download third button on the right of the table.");
			this.DocWithGivenUserAndWebDownload = analyzer.Analyse("I want to download a CSV file from the website 'https://www.regelleistung.net/apps/datacenter/tendering-files/?productTypes=FCR,aFRR,mFRR&markets=CAPACITY,ENERGY&fileTypes=DEMANDS,RESULTS,ANONYMOUS_LIST_OF_BIDS&dateRange=2022-04,2022-04' with the user 'ABC' and password 'pw123' by clicking on the download third button on the right of the table.");
		}

		[Test]
		public void AsureAnalysisDoesAnythingAnonymous()
		{
			Assert.IsNotNull(DocWithAnonymousUserAndWebDownload);

		}

		[Test]
		public void AsureAnalysisDoesAnything()
		{
			Assert.IsNotNull(DocWithGivenUserAndWebDownload);

		}

		[Test]
		public void AsureFTPServerIsRealisedAnonymous()
		{
			Assert.IsFalse(DocWithAnonymousUserAndWebDownload.IsFtpServerScript);
			Assert.IsTrue(DocWithAnonymousUserAndWebDownload.IsWebDownloadScript);
			Assert.AreEqual("https://www.regelleistung.net/apps/datacenter/tendering-files/?productTypes=FCR,aFRR,mFRR&markets=CAPACITY,ENERGY&fileTypes=DEMANDS,RESULTS,ANONYMOUS_LIST_OF_BIDS&dateRange=2022-04,2022-04", DocWithAnonymousUserAndWebDownload.WebSource.Url);
			Assert.IsNull(DocWithAnonymousUserAndWebDownload.WebSource.User);
			Assert.IsNull(DocWithAnonymousUserAndWebDownload.WebSource.Password);
			Assert.IsNotNull(DocWithAnonymousUserAndWebDownload.WebSource.DownloadLogic);
			Assert.AreEqual("clicking on the download third button on the right of the table", DocWithAnonymousUserAndWebDownload.WebSource.DownloadLogic);
		}

		[Test]
		public void AsureFTPServerIsRealisedGivenUser()
		{
			Assert.IsFalse(DocWithGivenUserAndWebDownload.IsFtpServerScript);
			Assert.IsTrue(DocWithGivenUserAndWebDownload.IsWebDownloadScript);
			Assert.AreEqual("https://www.regelleistung.net/apps/datacenter/tendering-files/?productTypes=FCR,aFRR,mFRR&markets=CAPACITY,ENERGY&fileTypes=DEMANDS,RESULTS,ANONYMOUS_LIST_OF_BIDS&dateRange=2022-04,2022-04", DocWithGivenUserAndWebDownload.WebSource.Url);
			Assert.IsNotNull(DocWithGivenUserAndWebDownload.WebSource.User);
			Assert.IsNotNull(DocWithGivenUserAndWebDownload.WebSource.Password);
			Assert.AreEqual("ABC", DocWithGivenUserAndWebDownload.WebSource.User);
			Assert.AreEqual("pw123", DocWithGivenUserAndWebDownload.WebSource.Password);
			Assert.IsNotNull(DocWithGivenUserAndWebDownload.WebSource.DownloadLogic);
			Assert.AreEqual("clicking on the download third button on the right of the table", DocWithGivenUserAndWebDownload.WebSource.DownloadLogic);
		}
	}
}
