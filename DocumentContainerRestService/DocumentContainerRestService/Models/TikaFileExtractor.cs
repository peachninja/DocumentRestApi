using DocumentContainerRestService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DocumentContainerRestService.Controllers;
using TikaOnDotNet.TextExtraction;

namespace DocumentContainerRestService.Models
{
    public class TikaFileExtractor : IFileExtractor
    {
        private TextExtractor TextExtractor { get; set; }
        public TextExtractionResult TextExtractionResult { get; set; }

        private readonly DocumentsController _docController;
        private readonly DocumentVersionsController _docVersionsController;
        public TikaFileExtractor()
        {
            TextExtractor = new TextExtractor();
            TextExtractionResult = new TextExtractionResult();
            _docController = new DocumentsController();
            _docVersionsController = new DocumentVersionsController();
        }
        public DocumentMetaData Extract(string path)
        {
            TextExtractionResult = TextExtractor.Extract(path);

            var doc = _docController.Create(TextExtractionResult);

            var docVersion = _docVersionsController.Create(doc, TextExtractionResult);
        
            DocumentMetaData docmeta = new DocumentMetaData
            {
                FilePath = TextExtractionResult.Metadata["FilePath"],
                Text = TextExtractionResult.Text,
                Document = doc,
                DocumentVersion = docVersion
            };

           
            return docmeta;
        }
    }
}