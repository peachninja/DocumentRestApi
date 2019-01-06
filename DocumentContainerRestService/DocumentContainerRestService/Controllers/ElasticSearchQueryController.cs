using DocumentContainerRestService.Interfaces;
using DocumentContainerRestService.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MongoDB.Bson;
using SearchRequest = System.DirectoryServices.Protocols.SearchRequest;

namespace DocumentContainerRestService.Controllers
{
    public class ElasticSearchQueryController : Controller
    {
        private ElasticSearchClient elClient;
        public ElasticSearchQueryController(ElasticSearchClient client)
        {
            this.elClient = client;
        }

        public void AddIndex(DocumentMetaData doc)
        {

            int id = MatchAll().Count;
            doc.Id = ++id;
            var indexResponse = elClient.Client.IndexDocument(doc);
            



        }

        public IReadOnlyCollection<DocumentMetaData> MatchAll()
        {
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s
               .MatchAll()
                );

            var documents = searchResponse.Documents;

            return documents;
        }
        public IReadOnlyCollection<DocumentMetaData> MatchCurrentVersion()
        {
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s
                .Source(sf => sf
                    .Includes(i => i
                        .Fields(
                            f => f.FilePath,
                            f => f.Document.Guid,
                            f => f.Text,
                            f => f.DocumentVersion.FileExtension,
                            f => f.DocumentVersion.DocumentPath


                        )
                    )
                 

                )
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Document.VersionStatus).Query(1.ToString())

                    )
                )
            );

            var documents = searchResponse.Documents;

            return documents;
        }
      
        public IReadOnlyCollection<DocumentMetaData> MatchById(int id)
        {
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s
         
                 .Query(q => q
                    .Match(m => m
                        .Field(f => f.Id).Query(id.ToString())

                 )
                )
                );

            var documents = searchResponse.Documents;

            return documents;
        }


        public IReadOnlyCollection<IDocumentMetaData> MatchAllDocumentVersionByDocId(string query)
        {
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s

                .Query(q => q
                    .Match(m => m
                        .Field(f => f.DocumentVersion.DocumentId).Query(query)

                    )
                )
            );

            var documents = searchResponse.Documents;

            return documents;
        }

        public IReadOnlyCollection<DocumentMetaData> MatchByCaseId(string caseguid)
        {
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s

                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Document.CaseId).Query(caseguid)

                    )
                )
            );

            var documents = searchResponse.Documents;

            return documents;
        }
        public IDocumentMetaData MatchNewestDocumentVersionByDocId(string query)
        {
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s

                .Query(q => q
                    .Match(m => m
                        .Field(f => f.DocumentVersion.DocumentId).Query(query)
                        
                    )
                )
                .Sort(q => q.Descending( t => t.DocumentVersion.VersionNumber))
            );

            var documents = searchResponse.Documents.FirstOrDefault();

            return documents;
        }
        public IReadOnlyCollection<IDocumentMetaData> MatchByText(string query)
        {
          
            var searchResponse = elClient.Client.Search<DocumentMetaData>(s => s
       .Query(q => q
        .MultiMatch(c => c
    .Fields(f => f.Field(p => p.Text).Field(p => p.Document.Title))
    .Query(query)
    .Analyzer("standard_danish")
    .Boost(1.1)
    .Slop(2)
    .Fuzziness(Fuzziness.Auto)
    .PrefixLength(2)
    .MaxExpansions(2)
    .Operator(Operator.Or)
    .MinimumShouldMatch(2)
    .FuzzyRewrite(MultiTermQueryRewrite.ConstantScoreBoolean)
    .TieBreaker(1.1)
    .CutoffFrequency(0.001)
    .Lenient()
    .ZeroTermsQuery(ZeroTermsQuery.All)
    .Name("find_text")
        )
         ));
            var documents = searchResponse.Documents;
            return documents;
        }

        public IUpdateResponse<DocumentMetaData> UpdateDocumentData(int id, DocumentMetaData data)
        {
            var getResponse = elClient.Client.Get<DocumentMetaData>(id, s => s .Index("document_test") .Type("document"));

            data = getResponse.Source;
            var response = elClient.Client.Update<DocumentMetaData>(data, s => s 
                .Doc(data)
            );

            return response;

        }

    }
}