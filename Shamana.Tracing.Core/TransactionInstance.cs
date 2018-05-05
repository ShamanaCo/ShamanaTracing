// <copyright file="TransactionInstance.cs" company="Shamana">
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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// Instance of web transaction.
    /// </summary>
    public class TransactionInstance
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="TransactionInstance"/> class from being created.
        /// </summary>
        private TransactionInstance()
        {
        }

        /// <summary>
        /// Occurs when detail begin.
        /// </summary>
        public event EventHandler<TransactionDetail> BeginDetail;

        /// <summary>
        /// Occurs when transaction begin.
        /// </summary>
        public event EventHandler<TransactionDescription> BeginTransaction;

        /// <summary>
        /// Occurs when detail end.
        /// </summary>
        public event EventHandler<TransactionDetail> EndDetail;

        /// <summary>
        /// Occurs when transaction end.
        /// </summary>
        public event EventHandler<TransactionDescription> EndTransaction;

        /// <summary>
        /// Gets the description of web transaction.
        /// </summary>
        /// <value>
        /// The description of web transaction.
        /// </value>
        public TransactionDescription Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the instance of transaction.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Instance of transaction.</returns>
        public static TransactionInstance GetInstance(HttpContext httpContext)
        {
            return TransactionInstance.GetInstance(new HttpContextWrapper(httpContext));
        }

        /// <summary>
        /// Gets the instance of transaction.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Instance of transaction.</returns>
        public static TransactionInstance GetInstance(HttpContextBase httpContext)
        {
            var instance = httpContext.Items[nameof(TransactionInstance)] as TransactionInstance;
            if (instance == null)
            {
                instance = new TransactionInstance();
                httpContext.Items[nameof(TransactionInstance)] = instance;
            }

            return instance;
        }

        /// <summary>
        /// Starts the detail transaction.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>Detail transaction.</returns>
        public TransactionDetail StartDetail(MethodBase methodInfo)
        {
            return this.StartDetail(
                TransactionHelper.GetPrettyFullName(methodInfo.ReflectedType),
                TransactionHelper.GetPrettyMethodName(methodInfo));
        }

        /// <summary>
        /// Starts the detail transaction.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>Detail transaction.</returns>
        public TransactionDetail StartDetail(string fullName, string methodName)
        {
            var detail = new TransactionDetail()
            {
                Id = this.Description.Id,
                FullName = fullName,
                MethodName = methodName
            };
            this.Description.Descriptions.Add(detail);
            this.BeginDetail?.Invoke(this, detail);
            return detail;
        }

        /// <summary>
        /// Starts the web transaction.
        /// </summary>
        /// <param name="requestUri">Uri of request.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>Web tansaction.</returns>
        public TransactionDescription StartTransaction(Uri requestUri, Type controllerType, string actionName)
        {
            return this.StartTransaction(
                requestUri,
                TransactionHelper.GetPrettyFullName(controllerType),
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(actionName));
        }

        /// <summary>
        /// Starts the transaction.
        /// </summary>
        /// <param name="requestUri">Uri of request.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>Web tansaction.</returns>
        public TransactionDescription StartTransaction(Uri requestUri, string fullName, string methodName)
        {
            this.Description = new TransactionDescription()
            {
                FullName = fullName,
                MethodName = methodName,
                Uri = requestUri
            };
            this.BeginTransaction?.Invoke(this, this.Description);
            return this.Description;
        }

        /// <summary>
        /// Stops the detail transaction.
        /// </summary>
        /// <param name="detail">The detail started transaction.</param>
        /// <param name="skipTreeTracing">Skip the tree tracing.</param>
        /// <returns>The detail stopped transaction.</returns>
        public TransactionDetail StopDetail(TransactionDetail detail, bool skipTreeTracing = false)
        {
            detail.Stopwatch.Stop();

            if (!skipTreeTracing)
            {
                int count = this.Description.Descriptions.Count - 2;
                if (count >= 0)
                {
                    this.Description.Descriptions.Skip(count).FirstOrDefault()?.Descriptions.Add(detail);
                    this.Description.Descriptions.Remove(detail);
                }
            }

            this.EndDetail?.Invoke(this, detail);
            return detail;
        }

        /// <summary>
        /// Stops the web transaction.
        /// </summary>
        /// <returns>Web tansaction.</returns>
        public TransactionDescription StopTransaction()
        {
            this.Description.Stopwatch.Stop();
            this.EndTransaction?.Invoke(this, this.Description);
            return this.Description;
        }
    }
}