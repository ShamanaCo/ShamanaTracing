// <copyright file="TransactionTabTest.cs" company="Shamana">
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
namespace Shamana.Tracing.Glimpse.Tests
{
    using System.IO;
    using System.Web;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shamana.Tracing.Core;

    /// <summary>
    /// Tests of transaction tab.
    /// </summary>
    [TestClass]
    public class TransactionTabTest
    {
        /// <summary>
        /// Test of the get data.
        /// </summary>
        [TestMethod]
        public void GetData()
        {
            var request = new HttpRequest(string.Empty, "http://localhost/", string.Empty);
            var stringWriter = new StringWriter();
            var response = new HttpResponse(stringWriter);
            HttpContext.Current = new HttpContext(request, response);
            TransactionInstance instance = TransactionInstance.GetInstance(HttpContext.Current);
            TransactionDescription description = instance.StartTransaction(request.Url, string.Empty, string.Empty);
            var transactionTab = new TransactionTab();
            var result = transactionTab.GetData(null) as TransactionDescription;
            Assert.AreEqual(result, description);
        }
    }
}