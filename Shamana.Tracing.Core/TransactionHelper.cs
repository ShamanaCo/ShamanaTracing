// <copyright file="TransactionHelper.cs" company="Shamana">
// Licensed to ShamanaTracing under one or more contributor
// license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright
// ownership. ShamanaTracing licenses this file to you under
// the Apache License, Version 2.0 (the "License"); you may
// not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
namespace Shamana.Tracing.Core
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Helper for the transactions.
    /// </summary>
    public static class TransactionHelper
    {
        /// <summary>
        /// Provider CSharp of CodeDom.
        /// </summary>
        private static readonly CodeDomProvider CsharpProvider = CodeDomProvider.CreateProvider("CSharp");

        /// <summary>
        /// The pretty full name cache.
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> PrettyFullNameCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// The pretty method name cache.
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> PrettyMethodNameCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Gets the full name and the method name.
        /// The Key is FullName and the Value is MethodName
        /// </summary>
        /// <param name="httpContext">The context HTTP.</param>
        /// <returns>The full name and method name.</returns>
        public static KeyValuePair<string, string> GetFullMethodName(HttpContext httpContext)
        {
            return TransactionHelper.GetFullMethodName(new HttpContextWrapper(httpContext));
        }

        /// <summary>
        /// Gets the full name and the method name.
        /// The Key is FullName and the Value is MethodName
        /// </summary>
        /// <param name="httpContext">The context HTTP.</param>
        /// <returns>The full name and method name.</returns>
        public static KeyValuePair<string, string> GetFullMethodName(HttpContextBase httpContext)
        {
            var result = new KeyValuePair<string, string>(string.Empty, string.Empty);

            RouteValueDictionary routes = httpContext.Request.RequestContext.RouteData.Values;
            if (routes.Keys.Contains("controller"))
            {
                string controller = routes["controller"] as string;
                string action = routes["action"] as string;
                result = new KeyValuePair<string, string>(
                     CultureInfo.CurrentCulture.TextInfo.ToTitleCase(controller) + "Controller",
                     CultureInfo.CurrentCulture.TextInfo.ToTitleCase(action ?? httpContext.Request.HttpMethod.ToLower()));
            }
            else if (httpContext.Handler != null)
            {
                Type type = httpContext.Handler.GetType();
                if (httpContext.Handler is System.Web.UI.Page)
                {
                    type = type.BaseType;
                }

                result = new KeyValuePair<string, string>(
                    TransactionHelper.GetPrettyFullName(type),
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(httpContext.Request.HttpMethod.ToLower()));
            }

            return result;
        }

        /// <summary>
        /// Gets the pretty full name of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Pretty full name.</returns>
        public static string GetPrettyFullName(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }
            else
            {
                return TransactionHelper.PrettyFullNameCache.GetOrAdd(
                    type.FullName,
                    (string key) =>
                    {
                        var reference = new CodeTypeReference(type);
                        return TransactionHelper.CsharpProvider.GetTypeOutput(reference);
                    });
            }
        }

        /// <summary>
        /// Gets the name of the pretty method.
        /// </summary>
        /// <param name="method">The infos of method.</param>
        /// <returns>Pretty method name.</returns>
        public static string GetPrettyMethodName(MethodBase method)
        {
            if (method == null)
            {
                return string.Empty;
            }
            else
            {
                string name = string.Concat(method.ReflectedType.FullName, '.', method.Name, '.', method.GetParameters().Length);
                return TransactionHelper.PrettyFullNameCache.GetOrAdd(
                    name,
                    (string key) =>
                    {
                        return method.ToString();
                    });
            }
        }
    }
}