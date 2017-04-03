// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestTests.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OwinMock.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    [TestClass]
    public class RestTests
    {
        [TestMethod]
        public async Task CanGetById()
        {
            var behavior = new MockedBehavior()
            {
                ExpectedJson = JsonConvert.SerializeObject(new Product() { Id = 100 }),
                HttpMethodName = HttpMethodNames.GET,
                ExpectedStatus = HttpStatusCode.OK,
                RequestPath = "/product/1"
            };

            using (new RestSvcMock("http://localhost:5000", new List<MockedBehavior>() { behavior }))
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://localhost:5000/product/1");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                var product = await response.GetResponseObjectAsync<Product>();
                Assert.IsNotNull(product);
                Assert.AreEqual(100, product.Id);
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
    }
}
