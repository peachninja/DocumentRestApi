using DocumentContainerRestService.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TikaOnDotNet.TextExtraction;

namespace DocumentContainerRestService.Models
{
    public class Document : IDocument
    {
        public int Id { get; set; }

        public Guid CaseId { get; set; }
        public string Title { get; set; }
        public short Status { get; set; }
        public string Owner { get; set; }
        public DocumentVersionStatus VersionStatus { get; set; }
        public Category Category { get; set; }
        public Guid Guid { get; set; }
        public string Text { get; set; }
        public string ContentType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }
        public string CheckedOut { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CheckedOutTime { get; set; }
     

     
    }
}