using DocumentContainerRestService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikaOnDotNet.TextExtraction;

namespace DocumentContainerRestService.Models
{
    public class Document : IDocument
    {
        public int Id { get; set; }

        public Guid CaseId { get; set; }
        public Guid Guid { get; set; }
        public string Text { get; set; }
        public string ContentType { get; set; }
        public IDictionary<string, string> Metadata { get; set; }

     
    }
}