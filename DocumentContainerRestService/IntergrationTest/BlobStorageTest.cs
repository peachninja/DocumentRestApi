using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using DocumentContainerRestService.Controllers;
using DocumentContainerRestService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace IntergrationTest
{
    /// <summary>
    /// Summary description for BlobStorageTest
    /// </summary>
    ///
    /// 
    [TestClass]

    public class BlobStorageTest
    {

        CloudStorageAccount cloudStorageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer blobContainer;
        BlobContainerPermissions containerPermissions;
        CloudBlob blob;

        private string connectionString = WebConfigurationManager.AppSettings["AzureBlobConnectionString"];
        public BlobStorageTest()
        {
            //
            // TODO: Add constructor logic here
            //
           
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void ConnectionToBlobTest()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            blobClient = cloudStorageAccount.CreateCloudBlobClient();

           
            blobContainer = blobClient.GetContainerReference("document");

            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = blobContainer.ListBlobsSegmented(null, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                Assert.AreEqual(3, results.Results.Count());
              
            } while (blobContinuationToken != null);
        }
    }
    
}
