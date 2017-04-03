// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpListenerTests.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OwinMock.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin.Hosting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Owin;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    [TestClass]
    public class HttpListenerTests
    {
        private IDisposable app;
        private const string BaseAddress = "http://localhost:5000";

        [TestInitialize]
        public void Setup()
        {
            app = WebApp.Start(BaseAddress, (app) =>
            {
                app.SetJsonMediaType();

                app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                {
                    if (env.Matches("/"))
                    {
                        await env.SetResponseAsync("Hello World");
                    }
                    else if (next != null)
                    {
                        await next.Invoke(env);
                    }
                })));
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (app != null)
            {
                app.Dispose();
                app = null;
            }
        }

        [TestMethod]
        public async Task Ping()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseAddress);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<string>(responseBody);
                Assert.AreEqual("Hello World", obj);

                var invalidRequestPath = new UriBuilder(BaseAddress) { Path = "Invalid" }.ToString();
                var response2 = await client.GetAsync(invalidRequestPath);
                Assert.AreEqual(HttpStatusCode.NotFound, response2.StatusCode);
            }
        }
    }
}
