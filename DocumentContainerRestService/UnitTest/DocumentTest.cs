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

      

    

        
        [TestMethod]
        public void DocumentToDatabaseTest()
        {
            

            using (var db = new DocumentContainerRestServiceContext())
            {
               
                try
                {
                 

                    Document doctest = new Document();
                    doctest.Id = 100;
                    doctest.CaseId = Guid.NewGuid();
                    doctest.Guid = Guid.NewGuid();
                    db.Documents.Add(doctest);
                    db.SaveChanges();
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
