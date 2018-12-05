﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentContainerRestService.Interfaces
{
   public interface IDocumentMetaData
    {

         int Id { get; set; }
         string ForeginKey { get; set; }
         string Text { get; set; }
         string ContentType { get; set; }
         IDictionary<string, string> Metadata { get; set; }

        string Url { get; set; }
    }
}