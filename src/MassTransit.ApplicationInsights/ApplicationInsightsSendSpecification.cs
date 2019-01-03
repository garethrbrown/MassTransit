﻿// Copyright 2007-2019 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GreenPipes;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    public class ApplicationInsightsSendSpecification<T> :
        IPipeSpecification<T>
        where T : class, SendContext
    {
        readonly Action<IOperationHolder<DependencyTelemetry>, T> _configureOperation;
        readonly TelemetryClient _telemetryClient;
        readonly string _telemetryHeaderParentKey;
        readonly string _telemetryHeaderRootKey;

        public ApplicationInsightsSendSpecification(TelemetryClient telemetryClient,
            Action<IOperationHolder<DependencyTelemetry>, T> configureOperation, string telemetryHeaderRootKey,
            string telemetryHeaderParentKey)
        {
            _telemetryClient = telemetryClient;
            _telemetryHeaderRootKey = telemetryHeaderRootKey;
            _telemetryHeaderParentKey = telemetryHeaderParentKey;
            _configureOperation = configureOperation;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(new ApplicationInsightsSendFilter<T>(_telemetryClient, _telemetryHeaderRootKey,
                _telemetryHeaderParentKey, _configureOperation));
        }
    }
}