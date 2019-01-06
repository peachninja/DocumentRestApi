using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DocumentContainerRestService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntergrationTest
{
    [TestClass]
    public class SqlDBTest
    {
        private string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        [TestMethod]
        public void SqlConnection()
        {
            using (var db = new DocumentContainerRestServiceContext())
            {
                var checkConnection = false;
                try
                {
                    
                    checkConnection = db.Database.Exists(); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                    
                }
                
                Assert.AreEqual(true, checkConnection);

            }
           
           
        }



   
    }
}
