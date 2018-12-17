using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentContainerRestService.Interfaces
{
    public interface IDocumentVersion
    {
        int Id { get; set; }
        Guid DocumentId { get; set; }
        int VersionNumber { get; set; }
        string DocumentPath { get; set; }
        string FileName { get; set; }
        string FileExtension { get; set; }
        DateTime LastUpdated { get; set; }
        string LastUpdatedBy { get; set; }

        int Size { get; set; }
        string Index { get; set; }


    }
}
