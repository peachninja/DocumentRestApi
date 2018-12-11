using DocumentContainerRestService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikaOnDotNet.TextExtraction;

namespace DocumentContainerRestService.Models
{
    public class TikkaFileExtractor : IFileExtractor
    {
        private TextExtractor TextExtractor { get; set; }
        private TextExtractionResult TextExtractionResult { get; set; }

        public TikkaFileExtractor()
        {
            TextExtractor = new TextExtractor();
            TextExtractionResult = new TextExtractionResult();
        }
        public DocumentMetaData Extract(string path)
        {
            IDocument doc = new Document
            {
                Guid = Guid.NewGuid()
              

            };

            DocumentMetaData docmeta = new DocumentMetaData
            {
                ForeginKey = doc.Guid.ToString(),
                Version = DocumentVersionStatus.Current
            };
            TextExtractionResult = TextExtractor.Extract(path);

            docmeta.Metadata = TextExtractionResult.Metadata;
            docmeta.Text = TextExtractionResult.Text;
            docmeta.ContentType = TextExtractionResult.ContentType;
            return docmeta;
        }
    }
}