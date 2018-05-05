// <copyright file="TransactionDetail.cs" company="Shamana">
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
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Detail of the transaction.
    /// </summary>
    public class TransactionDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetail"/> class.
        /// </summary>
        internal TransactionDetail()
        {
            this.Date = DateTime.Now;
            this.Descriptions = new HashSet<TransactionDetail>();
            this.Stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Gets the identifier of transaction.
        /// </summary>
        /// <value>
        /// The identifier of transaction.
        /// </value>
        public Guid Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the start date of the transaction.
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the duration of the transaction.
        /// </summary>
        public TimeSpan Elapsed
        {
            get
            {
                return this.Stopwatch.Elapsed;
            }
        }

        /// <summary>
        /// Gets the full name of the class.
        /// </summary>
        public string FullName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string MethodName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the details of the transaction.
        /// </summary>
        public IEnumerable<TransactionDetail> Details
        {
            get
            {
                return this.Descriptions.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets the descriptions of the transaction.
        /// </summary>
        /// <value>
        /// The descriptions of the transaction.
        /// </value>
        internal HashSet<TransactionDetail> Descriptions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the stopwatch of the transaction.
        /// </summary>
        /// <value>
        /// The stopwatch of the transaction.
        /// </value>
        internal Stopwatch Stopwatch
        {
            get;
            private set;
        }
    }
}