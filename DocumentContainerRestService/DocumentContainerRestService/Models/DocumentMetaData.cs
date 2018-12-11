using DocumentContainerRestService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentContainerRestService.Models
{
    public class DocumentMetaData : IDocumentMetaData
    {

        public int Id { get; set; }
        public string ForeginKey { get; set; }
        public string Text { get; set; }
        public string ContentType { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
        public string Url { get; set; }

        public DocumentVersionStatus Version { get; set; }
        public DocumentMetaData()
        {
           
        }
    }
}