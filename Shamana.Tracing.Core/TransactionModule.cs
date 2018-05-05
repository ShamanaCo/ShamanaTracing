// <copyright file="TransactionModule.cs" company="Shamana">
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
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Http module for manages web transactions.
    /// </summary>
    /// <seealso cref="IHttpModule" />
    public class TransactionModule : IHttpModule
    {
        /// <summary>
        /// Occurs when detail begin.
        /// </summary>
        public static event EventHandler<TransactionDetail> BeginDetail;

        /// <summary>
        /// Occurs when transaction begin.
        /// </summary>
        public static event EventHandler<TransactionDescription> BeginTransaction;

        /// <summary>
        /// Occurs when detail end.
        /// </summary>
        public static event EventHandler<TransactionDetail> EndDetail;

        /// <summary>
        /// Occurs when transaction end.
        /// </summary>
        public static event EventHandler<TransactionDescription> EndTransaction;

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
            {
                TransactionInstance transactionInstance = TransactionInstance.GetInstance(context.Context);
                transactionInstance.BeginDetail += TransactionModule.BeginDetail;
                transactionInstance.BeginTransaction += TransactionModule.BeginTransaction;
                transactionInstance.EndDetail += TransactionModule.EndDetail;
                transactionInstance.EndTransaction += TransactionModule.EndTransaction;
                context.Context.Items[nameof(TransactionModule)] = transactionInstance.StartTransaction(
                    context.Context.Request.Url,
                    nameof(HttpApplication),
                    nameof(TransactionModule));
            };

            context.AcquireRequestState += (sender, args) =>
            {
                context.Context.Items[nameof(TransactionDetail)] = TransactionInstance.GetInstance(context.Context).StartDetail(
                    nameof(HttpApplication),
                    nameof(context.AcquireRequestState));
            };
            context.PostAcquireRequestState += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopDetail(context.Context.Items[nameof(TransactionDetail)] as TransactionDetail, true);
                context.Context.Items.Remove(nameof(TransactionDetail));
            };

            context.AuthenticateRequest += (sender, args) =>
            {
                context.Context.Items[nameof(TransactionDetail)] = TransactionInstance.GetInstance(context.Context).StartDetail(
                    nameof(HttpApplication),
                    nameof(context.AuthenticateRequest));
            };
            context.PostAuthenticateRequest += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopDetail(
                    context.Context.Items[nameof(TransactionDetail)] as TransactionDetail,
                    true);
                context.Context.Items.Remove(nameof(TransactionDetail));
            };

            context.AuthorizeRequest += (sender, args) =>
            {
                context.Context.Items[nameof(TransactionDetail)] = TransactionInstance.GetInstance(context.Context).StartDetail(
                    nameof(HttpApplication),
                    nameof(context.AuthorizeRequest));
            };
            context.PostAuthorizeRequest += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopDetail(
                    context.Context.Items[nameof(TransactionDetail)] as TransactionDetail,
                    true);
                context.Context.Items.Remove(nameof(TransactionDetail));
            };

            context.ResolveRequestCache += (sender, args) =>
            {
                context.Context.Items[nameof(TransactionDetail)] = TransactionInstance.GetInstance(context.Context).StartDetail(
                    nameof(HttpApplication),
                    nameof(context.ResolveRequestCache));
            };
            context.PostResolveRequestCache += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopDetail(
                    context.Context.Items[nameof(TransactionDetail)] as TransactionDetail,
                    true);
                context.Context.Items.Remove(nameof(TransactionDetail));
            };

            context.PreRequestHandlerExecute += (sender, args) =>
            {
                if (context.Context.Items[nameof(TransactionModule)] is TransactionDescription transactionDescription)
                {
                    KeyValuePair<string, string> fullMethodName = TransactionHelper.GetFullMethodName(context.Context);
                    transactionDescription.FullName = fullMethodName.Key;
                    transactionDescription.MethodName = fullMethodName.Value;
                }

                context.Context.Items[nameof(TransactionDetail)] = TransactionInstance.GetInstance(context.Context).StartDetail(
                    nameof(HttpApplication),
                    nameof(context.Request));
            };
            context.PostRequestHandlerExecute += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopDetail(
                    context.Context.Items[nameof(TransactionDetail)] as TransactionDetail,
                    true);
                context.Context.Items.Remove(nameof(TransactionDetail));
            };

            context.UpdateRequestCache += (sender, args) =>
            {
                context.Context.Items[nameof(TransactionDetail)] = TransactionInstance.GetInstance(context.Context).StartDetail(
                    nameof(HttpApplication),
                    nameof(context.UpdateRequestCache));
            };
            context.PostUpdateRequestCache += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopDetail(
                    context.Context.Items[nameof(TransactionDetail)] as TransactionDetail,
                    true);
                context.Context.Items.Remove(nameof(TransactionDetail));
            };

            context.Error += (sender, args) =>
            {
                if (context.Context.Items[nameof(TransactionDetail)] is TransactionDetail transactionDetail)
                {
                    TransactionInstance.GetInstance(context.Context).StopDetail(
                        transactionDetail,
                        true);
                    context.Context.Items.Remove(nameof(TransactionDetail));
                }
            };

            context.EndRequest += (sender, args) =>
            {
                TransactionInstance.GetInstance(context.Context).StopTransaction();
            };
        }
    }
}