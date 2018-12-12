using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntergrationTest
{
    [TestClass]
    public class SqlDBTest
    {
        private string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=DocumentContainerRestServiceContext-20181122152647; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|DocumentContainerRestServiceContext-20181122152647.mdf";
        [TestMethod]
        public void SqlConnection()
        {
            bool checkConnection = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    checkConnection = true;

                }
                catch (SqlException)
                {
                    checkConnection = false;
                }

                Assert.AreEqual(true, checkConnection);
            }
        }
    }
}
