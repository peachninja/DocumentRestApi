using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using DocumentContainerRestService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace IntergrationTest
{
    /// <summary>
    /// Summary description for BlobStorageTest
    /// </summary>
    [TestClass]
    public class BlobStorageTest
    {
        CloudStorageAccount storageAccount = null;
        CloudBlobContainer cloudBlobContainer = null;

        string storageConnectionString =
            "DefaultEndpointsProtocol=https;AccountName=winx0007;AccountKey=Or7chC9Qt3N8D9/7lYICkIaiP3ksOfzrrP9IDuWXniW9ZDXcQnPQPzIOQJfnkKqXVr8hXKFct45tEN0IJCrPfQ==;EndpointSuffix=core.windows.net";
        string sourceFile = null;

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
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
             
                
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    
                    cloudBlobContainer = cloudBlobClient.GetContainerReference("quickstartblobs");
                     cloudBlobContainer.Create();
                      

                Assert.AreEqual("quickstartblobs", cloudBlobContainer.Name);
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                     cloudBlobContainer.SetPermissions(permissions);
                    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string localFileName = "QuickStart_" + ".txt";
                    sourceFile = Path.Combine(localPath, localFileName);
                    // Write text to the file.
                    File.WriteAllText(sourceFile, "Hello, World!");

                    BlobContinuationToken blobContinuationToken = null;
                    do
                    {
                        var results =  cloudBlobContainer.ListBlobsSegmented(null, blobContinuationToken);
                        // Get the value of the continuation token returned by the listing call.
                        blobContinuationToken = results.ContinuationToken;
                        Assert.AreEqual("https://winx0007.blob.core.windows.net/QuickStart_.txt", results.Results.FirstOrDefault().Uri.ToString());
                        Assert.AreEqual(1, results.Results.Count());
                    } while (blobContinuationToken != null);
                
               
                    if (cloudBlobContainer != null)
                    {
                         cloudBlobContainer.DeleteIfExists();
                      
                    }
                
                    File.Delete(sourceFile);
                    
                }
            }

        }
    
}
