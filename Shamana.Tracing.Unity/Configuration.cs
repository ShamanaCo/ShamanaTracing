// <copyright file="Configuration.cs" company="Shamana">
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
namespace Shamana.Tracing.Unity
{
    using System.Collections.Immutable;
    using global::Unity;
    using global::Unity.Interception.ContainerIntegration;
    using global::Unity.Interception.Interceptors.InstanceInterceptors.TransparentProxyInterception;
    using global::Unity.Interception.PolicyInjection;
    using global::Unity.Registration;

    /// <summary>
    /// Configuration for web transactions with Unity.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// The injection members for tracing with Interceptor[TransparentProxyInterceptor] and InterceptionBehavior[PolicyInjectionBehavior].
        /// </summary>
        public static readonly ImmutableArray<InjectionMember> InjectionTracing = new InjectionMember[]
        {
            new Interceptor<TransparentProxyInterceptor>(),
            new InterceptionBehavior<PolicyInjectionBehavior>()
        }.ToImmutableArray();

        /// <summary>
        /// Initialize interceptors for tracing.
        /// </summary>
        /// <param name="container">Unity Container.</param>
        /// <returns>Container.</returns>
        private static IUnityContainer RegisterTracing(this IUnityContainer container)
        {
            container
                .AddNewExtension<Interception>()
                .Configure<Interception>()
                .AddPolicy(nameof(TransactionCallHandler))
                .AddMatchingRule<TransactionMatchingRule>()
                .AddCallHandler<TransactionCallHandler>();
            return container;
        }
    }
}