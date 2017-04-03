// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequestTest.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RestMock.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class HttpRequestTest
    {
        private RestfulServiceMock app;

        [SetUp]
        public void Setup()
        {
            app = new RestfulServiceMock(5000);
        }

        [Test]
        public async Task TestGetExpectedStatus()
        {
            app.AddMock(new HttpResponseMock()
                .Get("hi"));
            app.Start();

            var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5000/hi");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
