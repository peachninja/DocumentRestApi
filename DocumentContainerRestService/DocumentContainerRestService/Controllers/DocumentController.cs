using DocumentContainerRestService.Interfaces;
using DocumentContainerRestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace DocumentContainerRestService.Controllers
{
    [RoutePrefix("api/document")]
    public class DocumentController : ApiController
    {

        private readonly ElasticSearchClient elclient;
        private ElasticSearchQueryController esQuery;
        private IFileExtractor extractor;
        public DocumentController()
        {
            string esendpoint = WebConfigurationManager.AppSettings["ElasticSearchEndpoint"];
            elclient = new ElasticSearchClient(esendpoint);
            esQuery = new ElasticSearchQueryController(elclient);
            extractor = new TikkaFileExtractor();
        }
        // GET: api/Document
        [Route("")]
        public IHttpActionResult GetAll()
        {
           
            var result = esQuery.MatchAll();
            if(result.Count < 1)
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
        // GET: api/Document/5
        [Route("search/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var result = esQuery.MatchById(id);
            if (result.Count < 1)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            else
            {
                return Ok(result);
            }
        }



        // GET: api/Document/text
        [Route("search/{text}")]
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
        // POST: api/Document
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]string value)
        {
            var doc = extractor.Extract(value);
            //esQuery.AddIndex(doc);
            if(doc != null)
            {
                return Ok(doc);
            }
            else
            {
                return Content(HttpStatusCode.NotAcceptable, doc);
            }

           
        }

        // PUT: api/Document/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Document/5
        public void Delete(int id)
        {
        }
    }
}
