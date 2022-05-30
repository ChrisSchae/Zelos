using ZelosFramework.FileHandling;
using NUnit.Framework;
using Persisting;
using Persisting.PostgreSQL;

namespace ZelosTest
{
    public class PersisterTest
    {
		public IScriptRepository repo { get; private set; }

		[SetUp]
		public void SetUp()
		{
			this.repo = new PostgreSQLRepo();
		}

		[Test]
		public void AsureConnectionToServerIsPossible()
		{
			var scriptResult = this.repo.GetScriptByName("Test123");

			Assert.IsNotNull(scriptResult);
			Assert.AreEqual(FileType.NONE, scriptResult.FileSettings.FileType);
			Assert.AreEqual("This is a script entry", scriptResult.ScriptString);
		}
	}
}
