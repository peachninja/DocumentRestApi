using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using sun.security.util;

namespace DocumentContainerRestService.Models
{
    public class AzureBlobStorage
    {
        public CloudStorageAccount StorageAccount;
        public CloudBlobContainer CloudBlobContainer;
        public CloudBlockBlob CloudBlockBlob;
        private readonly string _storageConnectionString = WebConfigurationManager.AppSettings["AzureBlobConnectionString"];

        public AzureBlobStorage()
        {
            if (CloudStorageAccount.TryParse(_storageConnectionString, out this.StorageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = StorageAccount.CreateCloudBlobClient();
                    this.CloudBlobContainer = cloudBlobClient.GetContainerReference("document");
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    this.CloudBlobContainer.SetPermissions(permissions);
                }
                catch (StorageException ex)
                {
                    Debug.println("Error returned from the service: {0}", ex.Message);
                }


            }
            else
            {
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named '_storageconnectionstring' with your storage " +
                    "connection string as a value.");
            }
        }
    }
}