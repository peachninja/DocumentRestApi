using System;
using System.Configuration;
using System.Data.SqlClient;
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
            bool checkConnection = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    checkConnection = true;
                    connection.Open();
                   

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
