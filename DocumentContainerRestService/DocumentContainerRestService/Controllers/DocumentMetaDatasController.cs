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
using DocumentContainerRestService.Interfaces;
using DocumentContainerRestService.Models;

namespace DocumentContainerRestService.Controllers
{
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
            extractor = new TikkaFileExtractor();
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
                return Ok(result);
            }

        }

        [Route("filepath")]
        // GET: api/Document/filepath
        public IHttpActionResult GetFilePath()
        {

            var result = esQuery.MatchAllFilePaths();
            if (result.Count < 1)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            else
            {
                return Ok(result);
            }

        }
        // GET: api/DocumentMetaDatas/5
        [Route("search/{id:int}", Name="GetDocDataById")]
        [ResponseType(typeof(DocumentMetaData))]
        public IHttpActionResult GetDocumentMetaData(int id)
        {
            var result = esQuery.MatchById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        // GET: api/DocumentMetaDatas/search/text
        [Route("searchtext/{text}")]
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
                return Ok(result);
            }
        }
        // PUT: api/DocumentMetaDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDocumentMetaData(int id, DocumentMetaData documentMetaData)
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
                db.SaveChanges();
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
            var newdoc = blobController.UploadFileToBlob(doc);
            esQuery.AddIndex(newdoc);
            string uri = Url.Link("GetDocDataById", new { id = newdoc.Id });
            response.Headers.Location = new Uri(uri);
            db.DocumentMetaDatas.Add(newdoc);
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