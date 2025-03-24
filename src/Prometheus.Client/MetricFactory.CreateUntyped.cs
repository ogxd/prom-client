using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public partial class MetricFactory : IMetricFactory
{
    public IUntyped CreateUntyped(string name, string help, bool includeTimestamp = false)
    {
        var metric = CreateUntyped(name, help, ValueTuple.Create(), includeTimestamp);
        return metric.Unlabelled;
    }

    public IMetricFamily<IUntyped, ValueTuple<string>> CreateUntyped(string name, string help, string labelName, bool includeTimestamp = false)
    {
        return CreateUntyped(name, help, ValueTuple.Create(labelName), includeTimestamp);
    }

    public IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<IUntyped, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
            metric = CreateUntypedInternal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<IUntyped> CreateUntyped(string name, string help, params string[] labelNames)
    {
        return CreateUntyped(name, help, false, labelNames);
    }

    public IMetricFamily<IUntyped> CreateUntyped(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        var metric = TryGetByName<IMetricFamily<IUntyped>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
            metric = GetUntypedFactory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

}
