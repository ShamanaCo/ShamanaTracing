// <copyright file="TransactionProxyTest.cs" company="Shamana">
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
    using System.Linq;
    using System.Web;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test of transparent proxy for transaction.
    /// </summary>
    [TestClass]
    public class TransactionProxyTest : HttpContextTest
    {
        /// <summary>
        /// Interface of test proxy.
        /// </summary>
        private interface IInterfaceProxyTest
        {
            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <returns>Name of interface.</returns>
            string GetName();
        }

        /// <summary>
        /// Test of creates the specified transparent proxy.
        /// </summary>
        [TestMethod]
        public void Create()
        {
            var interfaceProxy = TransactionProxy<IInterfaceProxyTest>.Create(HttpContext.Current, new InterfaceProxyTest());
            Assert.IsNotNull(interfaceProxy);
            interfaceProxy = TransactionProxy<IInterfaceProxyTest>.Create(new HttpContextWrapper(HttpContext.Current), new InterfaceProxyTest());
            Assert.IsNotNull(interfaceProxy);
            var marshalProxy = TransactionProxy<MarshalProxyTest>.Create(HttpContext.Current, new MarshalProxyTest());
            Assert.IsNotNull(marshalProxy);
            marshalProxy = TransactionProxy<MarshalProxyTest>.Create(new HttpContextWrapper(HttpContext.Current), new MarshalProxyTest());
            Assert.IsNotNull(marshalProxy);
        }

        /// <summary>
        /// Test of invokes this instance.
        /// </summary>
        [TestMethod]
        public void Invoke()
        {
            TransactionInstance instance = TransactionInstance.GetInstance(HttpContext.Current);

            var interfaceProxy = TransactionProxy<IInterfaceProxyTest>.Create(HttpContext.Current, new InterfaceProxyTest());
            string result = interfaceProxy.GetName();
            Assert.AreEqual(result, string.Empty);
            Assert.AreEqual(instance.Description.Details.Count(), 1);

            interfaceProxy = TransactionProxy<IInterfaceProxyTest>.Create(new HttpContextWrapper(HttpContext.Current), new InterfaceProxyTest());
            result = interfaceProxy.GetName();
            Assert.AreEqual(result, string.Empty);
            Assert.AreEqual(instance.Description.Details.First().Details.Count(), 1);

            var marshalProxy = TransactionProxy<MarshalProxyTest>.Create(HttpContext.Current, new MarshalProxyTest());
            result = marshalProxy.GetName();
            Assert.AreEqual(result, string.Empty);
            Assert.AreEqual(instance.Description.Details.First().Details.Count(), 2);

            marshalProxy = TransactionProxy<MarshalProxyTest>.Create(new HttpContextWrapper(HttpContext.Current), new MarshalProxyTest());
            result = marshalProxy.GetName();
            Assert.AreEqual(result, string.Empty);
            Assert.AreEqual(instance.Description.Details.First().Details.Count(), 3);
        }

        private class InterfaceProxyTest : IInterfaceProxyTest
        {
            public string GetName()
            {
                return string.Empty;
            }
        }

        private class MarshalProxyTest : MarshalByRefObject
        {
            public string GetName()
            {
                return string.Empty;
            }
        }
    }
}