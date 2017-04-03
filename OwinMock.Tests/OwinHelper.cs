// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwinHelper.cs" company="Microsoft Corporation">
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
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Owin;

    public static class OwinHelper
    {
        private const string OwinRequestPathKey = "owin.RequestPath";
        private const string OwinResponseBodyKey = "owin.ResponseBody";
        private const string OwinResponseStatusKey = "owin.ResponseStatusCode";
        private const string OwinRequestMethodKey = "owin.RequestMethod";

        public static bool Matches(this IDictionary<string, object> env, string requestPath)
        {
            if (env.ContainsKey(OwinRequestPathKey))
            {
                var path = env[OwinRequestPathKey] as string;
                if (path != null && path.Equals(requestPath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Matches(this IDictionary<string, object> env, MockedBehavior behavior)
        {
            if (env.ContainsKey(OwinRequestPathKey) && env.ContainsKey(OwinRequestMethodKey))
            {
                var path = env[OwinRequestPathKey] as string;
                var method = env[OwinRequestMethodKey] as string;
                var httpMethod = (HttpMethodNames) Enum.Parse(typeof(HttpMethodNames), method, true);

                if (path != null && 
                    path.Equals(behavior.RequestPath, StringComparison.OrdinalIgnoreCase) && 
                    httpMethod == behavior.HttpMethodName)
                {
                    return true;
                }
            }
            return false;
        }

        public static async Task SetResponseAsync<T>(this IDictionary<string, object> env, T value)
        {
            if (env.ContainsKey(OwinResponseBodyKey))
            {
                var stream = env[OwinResponseBodyKey] as Stream;
                if (stream != null)
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var json = JsonConvert.SerializeObject(value);
                        await writer.WriteAsync(json);
                    }
                }
            }
        }

        public static void SetStatusCode(this IDictionary<string, object> env, HttpStatusCode statusCode)
        {
            env[OwinResponseStatusKey] = ((int) statusCode).ToString();
        }

        public static void SetJsonMediaType(this IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            app.UseWebApi(config);
        }

        public static async Task<T> GetResponseObjectAsync<T>(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<string>(content);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
