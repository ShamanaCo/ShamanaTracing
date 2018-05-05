// <copyright file="Constants.cs" company="Shamana">
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
    using global::Glimpse.Core.Tab.Assist;

    /// <summary>
    /// Constants of library.
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// The transaction layout.
        /// </summary>
        internal static readonly object TransactionLayout = TabLayout
            .Create()
            .Cell("details", TabLayout
                .Create()
                .Row(r =>
                {
                    r.Cell("date").WithTitle("Date");
                    r.Cell("elapsed").AlignRight().WithTitle("Elapsed (ms)");
                    r.Cell("fullName").WithTitle("Full Name");
                    r.Cell("methodName").WithTitle("Method Name");
                    r.Cell("details").WithTitle("Details");
                }))
            .Build();
    }
}