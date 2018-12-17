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
        public string Text { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; }
        public Document Document { get; set; }
        public DocumentVersion DocumentVersion { get; set; }
    }
}