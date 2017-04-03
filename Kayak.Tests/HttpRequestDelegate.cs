namespace Kayak.Tests
{
    using System;
    using Kayak.Http;

    public class HttpRequestDelegate : IHttpRequestDelegate
    {
        #region Implementation of IHttpRequestDelegate

        public void OnRequest(HttpRequestHead head, IDataProducer body, IHttpResponseDelegate response)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}