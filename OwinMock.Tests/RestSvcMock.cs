// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestSvcMock.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OwinMock.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin.Hosting;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class RestSvcMock : IDisposable
    {
        private IDisposable app;
        private string apiEndpoint;
        private IList<MockedBehavior> mocks;

        public RestSvcMock(string baseAddress, IList<MockedBehavior> mockedBehaviors)
        {
            apiEndpoint = baseAddress;
            mocks = mockedBehaviors;
            StartListener();
        }

        private void StartListener()
        {
            app = WebApp.Start(apiEndpoint, (appBuilder) =>
            {
                appBuilder.SetJsonMediaType();
                
                appBuilder.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                {
                    MockedBehavior mockFound = mocks.FirstOrDefault(env.Matches);
                    if (mockFound != null)
                    {
                        env.Add("MockedBehavior", mockFound);
                        //env.SetStatusCode(mockFound.ExpectedStatus);
                        await env.SetResponseAsync(mockFound.ExpectedJson);
                    }
                    else if (next != null)
                    {
                        await next.Invoke(env);
                    }
                })));
            });
        }

        #region IDisposable

        public void Dispose()
        {
            if (app != null)
            {
                app.Dispose();
                app = null;
            }
        }

        #endregion
    }
}
