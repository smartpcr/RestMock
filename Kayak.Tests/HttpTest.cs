// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpTest.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kayak.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Kayak.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpTest
    {
        private const string BaseAddress = "http://localhost:5000";

        [TestInitialize]
        public void TestInitialize()
        {
            KayakServer.Factory.Create(new HttpServerDelegate(new HttpRequestDelegate()), new SchedulerDelegate());
        }
    }
}
