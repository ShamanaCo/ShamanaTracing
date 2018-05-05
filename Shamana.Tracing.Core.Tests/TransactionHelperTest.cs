// <copyright file="TransactionHelperTest.cs" company="Shamana">
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
namespace Shamana.Tracing.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests of helper for transactions.
    /// </summary>
    [TestClass]
    public class TransactionHelperTest
    {
        /// <summary>
        /// Test of get the full name of the method.
        /// </summary>
        [TestMethod]
        public void GetFullMethodName()
        {
            var request = new HttpRequest(string.Empty, "http://localhost/", string.Empty);
            request.RequestContext = new RequestContext();
            request.RequestContext.RouteData = new RouteData();
            request.RequestContext.RouteData.Values.Add("controller", "test");
            request.RequestContext.RouteData.Values.Add("action", "action");
            var stringWriter = new StringWriter();
            var response = new HttpResponse(stringWriter);
            HttpContext.Current = new HttpContext(request, response);
            KeyValuePair<string, string> result = TransactionHelper.GetFullMethodName(HttpContext.Current);
            Assert.AreEqual(result.Key, "TestController");
            Assert.AreEqual(result.Value, "Action");
            result = TransactionHelper.GetFullMethodName(new HttpContextWrapper(HttpContext.Current));
            Assert.AreEqual(result.Key, "TestController");
            Assert.AreEqual(result.Value, "Action");
            request.RequestContext.RouteData.Values.Remove("controller");
            HttpContext.Current.Handler = new TransactionHelperHandler();
            result = TransactionHelper.GetFullMethodName(HttpContext.Current);
            Assert.AreEqual(result.Key, "Shamana.Tracing.Core.Tests.TransactionHelperTest.TransactionHelperHandler");
            Assert.AreEqual(result.Value, "Get");
            result = TransactionHelper.GetFullMethodName(new HttpContextWrapper(HttpContext.Current));
            Assert.AreEqual(result.Key, "Shamana.Tracing.Core.Tests.TransactionHelperTest.TransactionHelperHandler");
            Assert.AreEqual(result.Value, "Get");
        }

        /// <summary>
        /// Test to get the pretty full name.
        /// </summary>
        [TestMethod]
        public void GetPrettyFullName()
        {
            Type type = typeof(TransactionHelperPretty<TransactionHelperTest>);
            string result = TransactionHelper.GetPrettyFullName(type);
            Assert.AreEqual(result, "Shamana.Tracing.Core.Tests.TransactionHelperTest.TransactionHelperPretty<Shamana.Tracing.Core.Tests.TransactionHelperTest>");
            result = TransactionHelper.GetPrettyFullName(null);
            Assert.AreEqual(result, string.Empty);
        }

        /// <summary>
        /// Test to get the name of the pretty method.
        /// </summary>
        [TestMethod]
        public void GetPrettyMethodName()
        {
            Type type = typeof(TransactionHelperPretty<TransactionHelperTest>);
            MethodInfo methodInfo = type.GetMethods().First(m => m.Name == "GetPrettyMethodName");
            string result = TransactionHelper.GetPrettyMethodName(methodInfo);
            Assert.AreEqual(result, methodInfo.ToString());
            result = TransactionHelper.GetPrettyMethodName(null);
            Assert.AreEqual(result, string.Empty);
        }

        private class TransactionHelperHandler : IHttpHandler
        {
            public bool IsReusable => false;

            public void ProcessRequest(HttpContext context)
            {
            }
        }

        private class TransactionHelperPretty<T>
        {
            public void GetPrettyMethodName<M>(string arg1, T arg2, M arg3)
            {
            }
        }
    }
}