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
    public class DocucmentVersionTest
    {
       
         
        private string pathTest = @"C:\Users\win.tin\Documents\testingblobupload.docx";

        [TestMethod]
        public void CreateDocumentVersionTest()
        {

            using (var db = new DocumentContainerRestServiceContext())
            {
              

                try
                {
                   

                    Document doctest = new Document();

                    doctest.Id = 100;
                    doctest.CaseId = Guid.NewGuid();
                    doctest.Guid = Guid.NewGuid();

                    DocumentVersion docVersionTest = new DocumentVersion();

                    docVersionTest.Id = 100;
                    docVersionTest.VersionNumber = 1;
                    docVersionTest.DocumentId = doctest.Guid;
                    db.Documents.Add(doctest);
                    db.DocumentVersions.Add(docVersionTest);

                    db.SaveChanges();

                    DocumentVersion docVersionToTest =
                        db.DocumentVersions.FirstOrDefault(a => a.Id == docVersionTest.Id);

                    Assert.IsNotNull(docVersionToTest);
                    Assert.AreEqual(docVersionToTest.DocumentId, docVersionTest.DocumentId);

                    
               

                    db.DocumentVersions.Remove(docVersionTest);
                    db.Documents.Remove(doctest);
                    db.SaveChanges();


                    Assert.IsNull(db.DocumentVersions.FirstOrDefault(a => a.Id == docVersionTest.Id));





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
