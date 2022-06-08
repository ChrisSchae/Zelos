using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZelosFramework.FileHandling;
using ZelosFramework.NLP_Core;
using ZelosFramework.NLP_Core.Catalyst;
using ZelosFramework.NLP_Core.FileSettings;

namespace ZelosTest
{
    public class NPLCoreTestFileSettings
    {
        private CatalystAnalyser analyzer;

        [SetUp]
		public void SetUp()
		{
			analyzer = new CatalystAnalyser();
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectZIP()
		{
			var DocWithZIP = this.analyzer.Analyse("I want to download a ZIP file from the FTP server 'xxx' from path 'yyyy' with anonymous user");
			Assert.AreEqual(FileType.ZIP, DocWithZIP.FileSettings.FileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectXML()
		{
			var DocWithXML = analyzer.Analyse("I want to download a XML file from the FTP server 'xxx' from path 'yyyy' with anonymous user");
			Assert.AreEqual(FileType.XML, DocWithXML.FileSettings.FileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectTXT()
		{
			var DocWithTXT = analyzer.Analyse("I want to download a TXT file from the FTP server 'xxx' from path 'yyyy' with anonymous user");
			Assert.AreEqual(FileType.TXT, DocWithTXT.FileSettings.FileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectPDF()
		{
			var DocWithPDF = analyzer.Analyse("I want to download a PDF file from the FTP server 'xxx' from path 'yyyy' with anonymous user");
			Assert.AreEqual(FileType.PDF, DocWithPDF.FileSettings.FileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectJSON()
		{
			var DocWithJSON = analyzer.Analyse("I want to download a JSON file from the FTP server 'xxx' from path 'yyyy' with anonymous user");
			Assert.AreEqual(FileType.JSON, DocWithJSON.FileSettings.FileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectExcel()
		{
			var DocWithEXCEL = analyzer.Analyse("I want to download a Excel file from the FTP server 'xxx' from path 'yyyy' with anonymous user");
			Assert.AreEqual(FileType.EXCEL, DocWithEXCEL.FileSettings.FileType);
		}

		[Test]
		public void AsureRecognizedFileTypeIsCorrectCSV()
		{
			var DocWithCSV = analyzer.Analyse("I want to download a CSV file from the FTP server 'xxx' from path 'yyyy' with anonymous user. The file has a header in row 2 and contains minimum 3 columns and maximum 4 columns the values are in columns 2 and 3");
			Assert.AreEqual(FileType.CSV, DocWithCSV.FileSettings.FileType);
			Assert.IsInstanceOf<CSVFileSetting>(DocWithCSV.FileSettings);
			Assert.AreEqual(3, ((CSVFileSetting)DocWithCSV.FileSettings).ExpectedMinCols);
			Assert.AreEqual(4, ((CSVFileSetting)DocWithCSV.FileSettings).ExpectedMaxCols);
			Assert.AreEqual(2, ((CSVFileSetting)DocWithCSV.FileSettings).HeaderRow); 
			int[] expectedValCols = { 2, 3 };
			Assert.AreEqual(expectedValCols, ((CSVFileSetting)DocWithCSV.FileSettings).ValueCols);
		}
	}
}
