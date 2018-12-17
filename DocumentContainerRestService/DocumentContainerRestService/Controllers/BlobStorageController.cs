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

        private readonly AzureBlobStorage _blob;
       
        

        public BlobStorageController()
        {
            this._blob = new AzureBlobStorage();
        }

        public DocumentMetaData UploadFileToBlob(DocumentMetaData data)
        {
            string filename = Path.GetFileNameWithoutExtension(data.FilePath);
            string ext = Path.GetExtension(data.FilePath);
            string blobReferencer = filename +"-caseid="+data.Document.CaseId+"-documentguid="+ data.Document.Guid+ "-version="+ data.Document.VersionStatus + ext;
            _blob.CloudBlockBlob = _blob.CloudBlobContainer.GetBlockBlobReference(blobReferencer);
            _blob.CloudBlockBlob.UploadFromFile(data.FilePath);
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results =  _blob.CloudBlobContainer.ListBlobsSegmented(blobReferencer, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                for (int i = 0; i < results.Results.Count(); i++)
                {
                    data.DocumentVersion.DocumentPath = results.Results.First().Uri.ToString();
                 
                }
             
            } while (blobContinuationToken != null);
            _blob.CloudBlockBlob.FetchAttributes();
            data.Size = (int)(_blob.CloudBlockBlob.Properties.Length);
            return data;

        }

        public void UploadNewVersion()
        {

        }
       public void DeleteFileFromBlob(DocumentMetaData data)
        {
            string filename = Path.GetFileNameWithoutExtension(data.FilePath);
            string ext = Path.GetExtension(data.FilePath);
            string blobReferencer = filename + "-caseid=" + data.Document.CaseId + "-documentguid=" + data.Document.Guid + "-version=" + data.Document.VersionStatus + ext;

            _blob.CloudBlockBlob = _blob.CloudBlobContainer.GetBlockBlobReference(blobReferencer);
            _blob.CloudBlockBlob.DeleteIfExistsAsync();
        }

        public void DownloadFile(string referencer)
        {
            _blob.CloudBlockBlob = _blob.CloudBlobContainer.GetBlockBlobReference(referencer);
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _blob.CloudBlockBlob.DownloadToFile(localPath, FileMode.Create);
        }
    }
}