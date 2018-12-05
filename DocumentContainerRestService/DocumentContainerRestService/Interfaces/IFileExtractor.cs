using DocumentContainerRestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentContainerRestService.Interfaces
{
    public interface IFileExtractor
    {
        DocumentMetaData Extract(string path);
    }
}
