// <copyright file="TransactionProxy.cs" company="Shamana">
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
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using System.Web;

    /// <summary>
    /// Transparent proxy for transaction.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <seealso cref="RealProxy" />
    public class TransactionProxy<T> : RealProxy
        where T : class
    {
        /// <summary>
        /// The decorated instance of type.
        /// </summary>
        private readonly T decorated = null;

        /// <summary>
        /// The HTTP context.
        /// </summary>
        private readonly HttpContextBase httpContext = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionProxy{T}"/> class.
        /// </summary>
        /// <param name="context">The HTTP Context.</param>
        /// <param name="instance">The instance of type.</param>
        private TransactionProxy(HttpContext context, T instance)
            : this(new HttpContextWrapper(context), instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionProxy{T}"/> class.
        /// </summary>
        /// <param name="context">The HTTP Context.</param>
        /// <param name="instance">The instance of type.</param>
        private TransactionProxy(HttpContextBase context, T instance)
            : base(typeof(T))
        {
            this.decorated = instance;
            this.httpContext = context;
        }

        /// <summary>
        /// Creates the specified transparent proxy.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>Transparent proxy.</returns>
        public static T Create(HttpContext context, T instance)
        {
            var proxy = new TransactionProxy<T>(context, instance);
            return proxy.GetTransparentProxy() as T;
        }

        /// <summary>
        /// Creates the specified transparent proxy.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>Transparent proxy.</returns>
        public static T Create(HttpContextBase context, T instance)
        {
            var proxy = new TransactionProxy<T>(context, instance);
            return proxy.GetTransparentProxy() as T;
        }

        /// <inheritdoc/>
        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IMethodCallMessage methodCall)
            {
                if (methodCall.MethodBase is MethodInfo methodInfo)
                {
                    TransactionInstance instance = TransactionInstance.GetInstance(this.httpContext);
                    TransactionDetail detail = instance.StartDetail(methodInfo);
                    try
                    {
                        object result = methodInfo.Invoke(this.decorated, methodCall.Args);
                        return new ReturnMessage(result, methodCall.Args, methodCall.Args.Length, methodCall.LogicalCallContext, methodCall);
                    }
                    finally
                    {
                        instance.StopDetail(detail);
                    }
                }
            }

            return msg;
        }
    }
}