using System;
using System.Text;
using System.Collections.Generic;
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
        
        DocumentMetaData testdoc = new DocumentMetaData();
        string path = @"C:\Users\win.tin\Documents\testdovc.txt";

        private string connectionString =
            "DefaultEndpointsProtocol=https;AccountName=winx0007;AccountKey=Or7chC9Qt3N8D9/7lYICkIaiP3ksOfzrrP9IDuWXniW9ZDXcQnPQPzIOQJfnkKqXVr8hXKFct45tEN0IJCrPfQ==;EndpointSuffix=core.windows.net";
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
            BlobStorage blobtest = new BlobStorage();
            Assert.IsNotNull(blobtest);
            testdoc.Metadata["FilePath"] = path;
            if (CloudStorageAccount.TryParse(connectionString, out blobtest.StorageAccount))
            {
                CloudBlobClient cloudBlobClient = blobtest.StorageAccount.CreateCloudBlobClient();
                blobtest.CloudBlobContainer = cloudBlobClient.GetContainerReference("testblobcontainer");
                blobtest.CloudBlobContainer.Create();
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                blobtest.CloudBlobContainer.SetPermissions(permissions);


                string filename = Path.GetFileNameWithoutExtension(testdoc.Metadata["FilePath"]);
              
                blobtest.CloudBlockBlob.UploadFromFile(testdoc.Metadata["FilePath"]);

                string desinationFolder = @"E:\test\blob\";

                blobtest.CloudBlockBlob.DownloadToFile(desinationFolder, FileMode.Create);
                string filenameDownloadedTest = Path.GetFileNameWithoutExtension(desinationFolder+filename);

                Assert.AreEqual(filename, filenameDownloadedTest);
            }

        }
    }
    
}
