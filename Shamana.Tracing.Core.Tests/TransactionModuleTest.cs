// <copyright file="TransactionModuleTest.cs" company="Shamana">
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
    using System.Web;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests of http module for manages web transactions.
    /// </summary>
    [TestClass]
    public class TransactionModuleTest
    {
        /// <summary>
        /// Test of initializes the module.
        /// </summary>
        [TestMethod]
        public void Init()
        {
            try
            {
                var application = new HttpApplication();
                var transaction = new TransactionModule();
                transaction.Init(application);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}