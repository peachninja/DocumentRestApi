using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DocumentContainerRestService.Models
{
    public class DocumentContainerRestServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DocumentContainerRestServiceContext() : base("name=DocumentContainerRestServiceContext")
        {
        }

        public System.Data.Entity.DbSet<DocumentContainerRestService.Models.DocumentMetaData> DocumentMetaDatas { get; set; }

        public System.Data.Entity.DbSet<DocumentContainerRestService.Models.Document> Documents { get; set; }

        public System.Data.Entity.DbSet<DocumentContainerRestService.Models.DocumentVersion> DocumentVersions { get; set; }
    }
}
