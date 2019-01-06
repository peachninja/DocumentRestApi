using System;
using System.Linq;
using DocumentContainerRestService.Controllers;
using DocumentContainerRestService.Interfaces;
using DocumentContainerRestService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class DocumentMetadataTest
    {
        readonly ElasticSearchClient testClient = new ElasticSearchClient("http://localhost:9200");
       

        [TestMethod]
        public void CreateDocumentMetaDataTest()
        {
            string path = @"C:\Users\win.tin\Documents\testdovc.txt";
            IFileExtractor testExtractor = new TikaFileExtractor();
            IDocumentMetaData testDoc =  testExtractor.Extract(path);
            Assert.IsInstanceOfType(testDoc, typeof(DocumentMetaData));
            Assert.IsNotNull(testDoc);
            Assert.AreEqual(path, testDoc.FilePath);
           // Assert.AreEqual(4, testDoc.Metadata.Count);
            string text = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nSprogpakker føjer flere værktøjer til skærm, hjælp eller korrektur til Office." +
                " Du kan installere ekstra tilbehørssprogpakker, når du har installeret Microsoft Office. Hvis en tilbehørssprogpakke beskrives" +
                " som delvist lokaliseret, vil nogle dele af Office muligvis stadigvæk blive vist på det sprog, som din kopi af Microsoft Office har.\r\r\n\r\r\nHvis " +
                "et sprog kun vises én gang, f.eks. tysk, så omfatter pakken værktøjer til alle lande/områder, der bruger dette sprog.\r\n";

            Assert.AreEqual(text, testDoc.Text);
        }


        [TestMethod]
        public void GetAllDocumentMetadDataTest()
        {

            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            Assert.IsNotNull(esQuery);
            var testDocuments = esQuery.MatchAll();
            int counttest = 2;
            Assert.AreEqual(counttest, testDocuments.Count);



        }

        [TestMethod]
        public void GetDocumentMetadDataByTextTest()
        {

            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            var testDocuments = esQuery.MatchByText("formpipe");
            int counttest = 0;
            var test_guid = "a2a67acb-5b11-4cae-8198-a0bcab98f5e6";
           // Assert.AreEqual(test_guid, testDocuments.ElementAt(0).ForeignKey);
            Assert.AreEqual(counttest, testDocuments.Count);

        }

        [TestMethod]
        public void GetDocumentMetadDataByIdTest()
        {

            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            var testDocuments = esQuery.MatchById(2);
            int counttest = 1;
            Assert.AreEqual(counttest, testDocuments.Count);

        }

        [TestMethod]
        public void GetDocumentMetadDataByVersionTest()
        {

            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            var testDocuments = esQuery.MatchCurrentVersion();
            int counttest = 0;
            Assert.AreEqual(counttest, testDocuments.Count);

        }


        [TestMethod]
        public void GetDocumentMetadDataByDocumentIdTest()
        {
            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            string guidTest = "1B2E0AE5-E92A-4CCB-89C7-A4ACB5EF93FD";
            var testDocuments = esQuery.MatchAllDocumentVersionByDocId(guidTest);
            int counttest = 1;
            Assert.AreEqual(counttest, testDocuments.Count);
            string filenameTest = "Calculator v4";
            Assert.AreEqual(filenameTest, testDocuments.FirstOrDefault().DocumentVersion.FileName);
        }
        

        [TestMethod]
        public void GetNewestDocumentMetadDataByDocumentIdTest()
        {

            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            string guidTest = "1B2E0AE5-E92A-4CCB-89C7-A4ACB5EF93FD";
            var testDocuments = esQuery.MatchNewestDocumentVersionByDocId(guidTest);
            
            Assert.AreEqual(1, testDocuments.DocumentVersion.VersionNumber);

        }

        [TestMethod]
        public void GetDocumentMetadDataByCaseIdTest()
        {

            ElasticSearchQueryController esQuery = new ElasticSearchQueryController(testClient);
            string guidTest = "9898ac0e-4d2c-4949-84de-d79e4e62e187";
            var testDocuments = esQuery.MatchByCaseId(guidTest);
            int counttest = 1;
            Assert.AreEqual(counttest, testDocuments.Count);

        }
    }
}
