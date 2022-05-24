using ZelosFramework.FTP;
using NUnit.Framework;

namespace ZelosTest
{
    public class FtpComponentTest
    {
        public FtpComponent FTP { get; private set; }

        [SetUp]
		public void SetUp()
		{
			this.FTP = new FtpComponent("ftp.dlptest.com", "dlpuser", "rNrKYTX9g7z3RgJRmxWuGHbeu");
		}

		[Test]
		public void AsureConnectionToServerIsPossible()
		{
			this.FTP.Connect();

			Assert.IsTrue(this.FTP.IsConnected());
		}
	}
}
