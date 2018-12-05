using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentContainerRestService.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DocumentContainerRestService.Controllers
{
    public class BlobStorageController : Controller
    {

        private BlobStorage blob;

        public BlobStorageController()
        {
            this.blob = new BlobStorage();
        }

        public DocumentMetaData UploadFileToBlob(DocumentMetaData data)
        {
            string filename = Path.GetFileNameWithoutExtension(data.Metadata["FilePath"]);
            string ext = Path.GetExtension(data.Metadata["FilePath"]);
            string blobreferencer = filename + data.ForeginKey + ext;
            CloudBlockBlob cloudBlockBlob = blob.CloudBlobContainer.GetBlockBlobReference(blobreferencer);
            cloudBlockBlob.UploadFromFile(data.Metadata["FilePath"]);
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results =  blob.CloudBlobContainer.ListBlobsSegmented(blobreferencer, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                for (int i = 0; i < results.Results.Count(); i++)
                {
                    data.Url = results.Results.First().Uri.ToString();
                }
             
            } while (blobContinuationToken != null);

            return data;

        }
    }
}