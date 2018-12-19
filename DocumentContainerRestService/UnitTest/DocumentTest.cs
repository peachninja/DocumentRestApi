using System;
using System.Configuration;
using System.IO;
using System.Linq;
using DocumentContainerRestService.Controllers;
using DocumentContainerRestService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TikaOnDotNet.TextExtraction;

namespace UnitTest
{
    [TestClass]
    public class DocumentTest
    {
        TikaFileExtractor extractorTest = new TikaFileExtractor();
        DocumentsController test_controller = new DocumentsController();
        private string pathTest = @"C:\Users\win.tin\Documents\testingblobupload.docx";

    

        
        [TestMethod]
        public void DocumentToDatabaseTest()
        {
            

            using (var db = new DocumentContainerRestServiceContext())
            {
               
                try
                {
                    extractorTest.TextExtractionResult = extractorTest.TextExtractor.Extract(pathTest);

                    Document doctest = test_controller.Create(extractorTest.TextExtractionResult);
                    Document docToTest = db.Documents.FirstOrDefault(a => a.Id == doctest.Id);
                    
                    Assert.IsNotNull(docToTest);

                    Assert.AreEqual(docToTest.Guid, doctest.Guid);

                    Assert.AreEqual(docToTest.Owner, doctest.Owner);
                    Assert.AreEqual(docToTest.Created, doctest.Created);
                    Assert.AreEqual(docToTest.CaseId, doctest.CaseId);

                 
                    db.Documents.Remove(doctest);
                    db.SaveChanges();

                    Assert.IsNull(db.Documents.FirstOrDefault(a => a.Id == doctest.Id));



                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;

                }

               

            }
          

          

        }
     

    }
}
