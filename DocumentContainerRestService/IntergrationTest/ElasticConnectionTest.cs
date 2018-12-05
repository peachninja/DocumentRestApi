using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using DocumentContainerRestService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using Elasticsearch.Net;
using RestSharp;
using System.Net;

namespace IntergrationTest
{
    [TestClass]
    public class ElasticConnectionTest
    {

        ElasticSearchClient testClient = new ElasticSearchClient("http://localhost:9200");
        RestClient client = new RestClient("http://localhost:9200");

        [TestMethod]
        public void GetConnectionTest()
        {
          
            Assert.IsNotNull(testClient);
        
           
            
            var request = new RestRequest("/", Method.GET);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string testname = "DEVWINTIN";
            Assert.AreEqual(true, content.Contains(testname));



        }


        [TestMethod]
        public void CreateIndexTest()
        {
            testClient.Client.CreateIndex("testindex");

            var request = new RestRequest("/testindex", Method.GET);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);


        }



        [TestMethod]
        public void AddToIndexTest()
        {
            var person = new Person
            {
                Id = 1,
                FirstName = "Martijn",
                LastName = "Laarman"
            };

            var response = testClient.Client.Index(person, idx => idx.Index("testindex"));


            Assert.AreEqual(true, response.IsValid);


        }
        [TestMethod]
        public void GetIndexTest()
        {
            var request = new RestRequest("/testindex/_search", Method.POST);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

           
           
        }
        [TestMethod]
        public void DeleteIndexTest()
        {
            var request = new RestRequest("/testindex", Method.DELETE);

            IRestResponse response = client.Execute(request);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


       
    }
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


}
