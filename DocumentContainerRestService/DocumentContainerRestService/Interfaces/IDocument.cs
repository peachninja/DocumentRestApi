using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentContainerRestService.Models;

namespace DocumentContainerRestService.Interfaces
{
    public interface IDocument
    {
         int Id { get; set; }
         Guid CaseId { get; set; }
         string Title { get; set; }
         Int16 Status { get; set; }
         string Owner { get; set; }
         DocumentVersionStatus VersionStatus { get; set; }
         Category Category { get; set; }
         Guid Guid { get; set; }
         string Text { get; set; }
         string ContentType { get; set; }

         DateTime Created { get; set; }
         
         string CheckedOut { get; set; }
         
         DateTime CheckedOutTime { get; set; }

         
    }
}
