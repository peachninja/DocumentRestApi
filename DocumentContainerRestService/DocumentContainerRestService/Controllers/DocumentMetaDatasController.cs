using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using DocumentContainerRestService.Filters;
using DocumentContainerRestService.Interfaces;
using DocumentContainerRestService.Models;

namespace DocumentContainerRestService.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/documentmetadata")]
    public class DocumentMetaDatasController : ApiController
    {
        private DocumentContainerRestServiceContext db = new DocumentContainerRestServiceContext();
        private readonly ElasticSearchClient elclient;
        private ElasticSearchQueryController esQuery;
        private readonly IFileExtractor extractor;
        private readonly BlobStorageController blobController;

        public DocumentMetaDatasController()
        {
            string esendpoint = WebConfigurationManager.AppSettings["ElasticSearchEndpoint"];
            elclient = new ElasticSearchClient(esendpoint);
            esQuery = new ElasticSearchQueryController(elclient);
            extractor = new TikaFileExtractor();
            blobController = new BlobStorageController();
        }
        // GET: api/DocumentMetaDatas
       
        [Route("")]
        public IHttpActionResult GetAll()
        {

            var result = esQuery.MatchAll();
            if (result.Count < 1)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            else
            {

                 List<string> urlList = new List<string>();
                foreach (var i in result)
                {
                    urlList.Add(i.DocumentVersion.DocumentPath);
                }
                return Ok(urlList);
            }

        }
        [Route("current")]
        public IHttpActionResult GetAllCurrentVersionDocument()
        {

            var result = esQuery.MatchCurrentVersion();
            if (result.Count < 1)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            else
            {

                List<string> urlList = new List<string>();
                foreach (var i in result)
                {
                    urlList.Add(i.DocumentVersion.DocumentPath);
                }
                return Ok(urlList);
            }
        }
        // GET: api/DocumentMetaDatas/5
        [Route("id/{id:int}", Name="GetDocDataById")]
        [ResponseType(typeof(DocumentMetaData))]
        public IHttpActionResult GetDocumentMetaData(int id)
        {

            var result = esQuery.MatchById(id);
            if (result == null)
            {
                return NotFound();
            }

            List<string> urlList = new List<string>();
            foreach (var i in result)
            {
                urlList.Add(i.DocumentVersion.DocumentPath);
            }
            return Ok(urlList);
        }
        // GET: api/DocumentMetaDatas/search/text
        [Route("text/{text}")]
        [HttpGet]
        public IHttpActionResult GetByText(string text)
        {

            var result = esQuery.MatchByText(text);

            if (result.Count < 1)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            else
            {
                List<string> urlList = new List<string>();
                foreach (var i in result)
                {
                    urlList.Add(i.DocumentVersion.DocumentPath);
                }
                return Ok(urlList);
            }
        }

        // GET: api/DocumentMetaDatas/search/text
        [Route("documentid/{guid}")]
        [HttpGet]
        public IHttpActionResult GetAllDocumentVersionForDocumentByDocumentId(string guid)
        {

            var result = esQuery.MatchAllDocumentVersionByDocId(guid);

            if (result.Count < 1)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            else
            {
                List<string> urlList = new List<string>();
                foreach (var i in result)
                {
                    urlList.Add(i.DocumentVersion.DocumentPath);
                }
                return Ok(urlList);
            }
        }

        // GET: api/DocumentMetaDatas/search/text
        [Route("documentid/newest/{guid}")]
        [HttpGet]
        public IHttpActionResult GetNewestDocumentVersionForDocumentByDocumentId(string guid)
        {

            var result = esQuery.MatchNewestDocumentVersionByDocId(guid);

            if (result != null)
            {
                return Ok(result.DocumentVersion.DocumentPath);
            }
            else
            {
                return Content(HttpStatusCode.NoContent, "No result");
            }
          
            
        }
        // PUT: api/DocumentMetaDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocumentMetaData(int id, [FromBody] DocumentMetaData documentMetaData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != documentMetaData.Id)
            {
                return BadRequest();
            }

            db.Entry(documentMetaData).State = EntityState.Modified;

            try
            {
               var response = esQuery.UpdateDocumentData(id, documentMetaData);
                db.SaveChanges();
                return Ok(response);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentMetaDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DocumentMetaDatas
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DocumentMetaData))]
        public HttpResponseMessage PostDocumentMetaData([FromBody]string value)
        {
            var response = Request.CreateResponse(HttpStatusCode.Created);
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var doc = extractor.Extract(value);
            DocumentMetaData newdoc = blobController.UploadFileToBlob(doc);
            esQuery.AddIndex(newdoc);
            string uri = Url.Link("GetDocDataById", new { id = newdoc.Id });
            response.Headers.Location = new Uri(uri);
            db.DocumentMetaDatas.Add(newdoc);
            db.DocumentVersions.Find(doc.DocumentVersion.Id).Size = newdoc.Size;
            db.SaveChanges();

            return response;
        }

        // DELETE: api/DocumentMetaDatas/5
        [ResponseType(typeof(DocumentMetaData))]
        public IHttpActionResult DeleteDocumentMetaData(int id)
        {
            DocumentMetaData documentMetaData = db.DocumentMetaDatas.Find(id);
            if (documentMetaData == null)
            {
                return NotFound();
            }

            db.DocumentMetaDatas.Remove(documentMetaData);
            db.SaveChanges();

            return Ok(documentMetaData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentMetaDataExists(int id)
        {
            return db.DocumentMetaDatas.Count(e => e.Id == id) > 0;
        }
    }
}