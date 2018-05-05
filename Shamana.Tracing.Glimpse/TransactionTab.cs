// <copyright file="TransactionTab.cs" company="Shamana">
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
namespace Shamana.Tracing.Glimpse
{
    using System.Web;
    using global::Glimpse.AspNet.Extensibility;
    using global::Glimpse.Core.Extensibility;
    using Shamana.Tracing.Core;

    /// <summary>
    /// Tab of current web transaction.
    /// </summary>
    /// <seealso cref="AspNetTab" />
    /// <seealso cref="ITabLayout" />
    public class TransactionTab : AspNetTab, ITabLayout
    {
        /// <summary>
        /// Gets the execute on.
        /// </summary>
        /// <value>
        /// The execute on.
        /// </value>
        public override RuntimeEvent ExecuteOn => RuntimeEvent.EndRequest;

        /// <summary>
        /// Gets the name of tab.
        /// </summary>
        /// <value>
        /// The name of tab.
        /// </value>
        public override string Name => "Transaction";

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Web transation datas.</returns>
        public override object GetData(ITabContext context)
        {
            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            TransactionInstance transactionInstance = TransactionInstance.GetInstance(httpContext);
            return transactionInstance.Description;
        }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        /// <returns>Layout datas.</returns>
        public object GetLayout()
        {
            return Constants.TransactionLayout;
        }
    }
}