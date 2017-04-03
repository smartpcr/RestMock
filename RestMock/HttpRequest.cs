// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequest.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RestMock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public enum HttpVerbs
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum HttpContentTypes
    {
        text,
        json,
        xml
    }

    public enum HttpAccepts
    {
        text,
        json,
        xml
    }

    public class HttpRequest
    {
        public HttpVerbs Verb { get; set; }
        public HttpAccepts Accepts { get; set; }
        public HttpContentTypes ContentType { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }

        public bool Matches(HttpContext context)
        {
            return context.Request.Path.Equals(Url, StringComparison.OrdinalIgnoreCase) &&
                   context.Request.Method.Equals(Verb.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
