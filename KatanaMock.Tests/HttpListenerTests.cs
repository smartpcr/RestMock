// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpListenerTests.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace KatanaMock.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Owin.Host.HttpListener;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    [TestClass]
    public class HttpListenerTests
    {
        [TestInitialize]
        public void Setup()
        {
            OwinHttpListener listener = null;
        }


        private OwinHttpListener CreateServer(AppFunc app, string[] addressParts)
        {
            var wrapper = new OwinHttpListener();
            wrapper.Start(wrapper.Listener, app, CreateAddress(addressParts), null, null);
            return wrapper;
        }

        private static IList<IDictionary<string, object>> CreateAddress(string[] addressParts)
        {
            var address = new Dictionary<string, object>();
            address["scheme"] = addressParts[0];
            address["host"] = addressParts[1];
            address["port"] = addressParts[2];
            address["path"] = addressParts[3];

            IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            list.Add(address);
            return list;
        }
    }
}
