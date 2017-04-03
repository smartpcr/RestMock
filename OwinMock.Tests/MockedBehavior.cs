// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockedBehavior.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OwinMock.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    public class MockedBehavior
    {
        public HttpMethodNames HttpMethodName { get; set; }
        public string RequestPath { get; set; }
        public HttpStatusCode ExpectedStatus { get; set; }
        public string ExpectedJson { get; set; }
    }

    public enum HttpMethodNames
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
