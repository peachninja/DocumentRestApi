using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentContainerRestService.Models;

namespace DocumentContainerRestService.Interfaces
{
   public interface IDocumentMetaData
    {

         int Id { get; set; }
        string Text { get; set; }

        string FilePath { get; set; }
        int Size { get; set; }
         Document Document { get; set; }
         DocumentVersion DocumentVersion { get; set; }
    }
}
