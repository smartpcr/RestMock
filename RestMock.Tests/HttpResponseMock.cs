// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpResponseMock.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RestMock
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class HttpResponseMock
    {
        public HttpRequest Request { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public HttpContentTypes ContentType { get; private set; }

        public HttpResponseMock(HttpRequest request)
        {
            Request = request;
        }

        public HttpResponseMock()
        {
            Request = new HttpRequest();
        }
        
        public HttpResponseMock SetContentType(HttpContentTypes contentType)
        {
            Request.ContentType = contentType;
            return this;
        }

        public HttpResponseMock Get(string url)
        {
            Request.Verb = HttpVerbs.GET;
            Request.Url = url;
            return this;
        }

        public HttpResponseMock Post(string url)
        {
            Request.Verb = HttpVerbs.POST;
            Request.Url = url;
            return this;
        }

        public HttpResponseMock Put(string url)
        {
            Request.Verb = HttpVerbs.PUT;
            Request.Url = url;
            return this;
        }

        public HttpResponseMock Delete(string url)
        {
            Request.Verb = HttpVerbs.DELETE;
            Request.Url = url;
            return this;
        }

        public HttpResponseMock Expect(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public HttpResponseMock Expect(HttpContentTypes contentType)
        {
            ContentType = contentType;
            return this;
        }

        public Task SetResponse(HttpContext context)
        {
            return context.Response.WriteAsync("");
        }
    }
}
