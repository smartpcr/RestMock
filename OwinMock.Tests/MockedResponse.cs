// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockedResponse.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OwinMock.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class MockedResponse : OwinMiddleware
    {
        private MockedBehavior _behavior;

        public MockedResponse(OwinMiddleware next) : base(next)
        {
        }

        #region Overrides of OwinMiddleware

        public override async Task Invoke(IOwinContext context)
        {
            _behavior = context.Environment["MockedBehavior"] as MockedBehavior;
            if (_behavior != null)
            {
                var response = context.Response;
                response.OnSendingHeaders(async state =>
                {
                    var resp = (OwinResponse)state;
                    resp.StatusCode = (int)_behavior.ExpectedStatus;

                    if (_behavior.ExpectedJson != null)
                    {
                        using (var writer = new StreamWriter(resp.Body))
                        {
                            await writer.WriteAsync(_behavior.ExpectedJson);
                        }
                    }
                }, response);
            }

            await Next.Invoke(context);
        }

        #endregion
    }
}
