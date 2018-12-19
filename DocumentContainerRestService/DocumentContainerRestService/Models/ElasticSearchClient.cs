using Elasticsearch.Net;
using Nest;
using Nest.JsonNetSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentContainerRestService.Models
{
    public class ElasticSearchClient
    {
        public ElasticClient Client { get; set; }

        public ElasticSearchClient(string endPoint)
        {
            var connectionSettings = new ConnectionSettings().DefaultIndex("document_test").DefaultTypeName("document");
           
            

            this.Client = new ElasticClient(connectionSettings);
            Client.CreateIndex("document_test", c => c
              .Settings(s => s
        .Analysis(a => a
        
            .Analyzers(aa => aa
               .Custom("standard_danish", ca => ca
                   
                    .Tokenizer("standard")
                    .Filters("danish_keywords", "lowercase", "danish_stop", "danish_stemmer")
                )

            )
            
        )
    )
    .Mappings(m => m
        .Map<DocumentMetaData>(mm => mm
            .Properties(p => p
                .Text(t => t
                    .Name(n => n.Text)
                    
                    .Analyzer("standard_danish")
                ).Text(t => t 
                .Name(n => n.Metadata["Title"])
                .Analyzer("standard_danish")
                )
            
            )
        )
    )
);

        }



    }
}