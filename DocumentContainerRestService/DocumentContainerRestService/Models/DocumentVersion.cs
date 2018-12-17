using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DocumentContainerRestService.Interfaces;

namespace DocumentContainerRestService.Models
{
    public class DocumentVersion : IDocumentVersion
    {
        public int Id { get; set; }
        public Guid DocumentId { get; set; }
        public int VersionNumber { get; set; }
        public string DocumentPath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }
        public int Size { get; set; }
        public string Index { get; set; }
    }
}