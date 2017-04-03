// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestfulServiceMock.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RestMock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// create HTTP/HTTPS listener on localhost
    /// </summary>
    public class RestfulServiceMock : IDisposable
    {
        private static readonly IPAddress HostAddress = IPAddress.Loopback;
        private static readonly IList<HttpResponseMock> Mocks = new List<HttpResponseMock>();

        #region fields  
        private IWebHost host;
        #endregion

        #region props 
        public int Port { get; private set; }
        public bool UseSsl { get; private set; }
        public X509Certificate2 SslCert { get; private set; }
        
        #endregion 

        #region ctor 

        public RestfulServiceMock()
            : this(80)
        {
        }

        public RestfulServiceMock(int port)
        {
            Port = port;
            Init();
        }

        public RestfulServiceMock(X509Certificate2 sslCert)
        {
            UseSsl = true;
            SslCert = sslCert;
            Port = 443;

            Init();
        }
        #endregion
        
        private void Init()
        {
            host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.Listen(new IPEndPoint(HostAddress, Port), listenOptions =>
                    {
                        if (UseSsl)
                        {
                            listenOptions.UseHttps(SslCert);
                        }
                    });
                })
                .Configure(app =>
                {
                    app.Run(context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.OK;

                        foreach (var mock in Mocks)
                        {
                            if (mock.Request.Matches(context))
                            {
                                return mock.SetResponse(context);
                            }
                        }

                        return context.Response.WriteAsync("Not matched: " + context.Request.Path);
                    });
                })
                .Build();
        }
        
        public void AddMock(HttpResponseMock mock)
        {
            Mocks.Add(mock);
        }

        public void Start()
        {
            Dispose();
            Init();
            host.Start();
        }
        
        public void Dispose()
        {
            if (host != null)
            {
                host.Dispose();
                host = null;
            }
        }
    }
}
