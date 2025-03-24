using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public partial class MetricFactory : IMetricFactory
{
    public ICounter CreateCounter(string name, string help, bool includeTimestamp = false)
    {
        var metric = CreateCounter(name, help, ValueTuple.Create(), includeTimestamp);
        return metric.Unlabelled;
    }

    public IMetricFamily<ICounter, ValueTuple<string>> CreateCounter(string name, string help, string labelName, bool includeTimestamp = false)
    {
        return CreateCounter(name, help, ValueTuple.Create(labelName), includeTimestamp);
    }


    public IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return CreateCounter(name, help, labelNames, includeTimestamp);
    }

    public IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<ICounter, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp, timeToLive);
            metric = CreateCounterInternal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<ICounter> CreateCounter(string name, string help, params string[] labelNames)
    {
        return CreateCounter(name, help, false, labelNames);
    }

    public IMetricFamily<ICounter> CreateCounter(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        var metric = TryGetByName<IMetricFamily<ICounter>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
            metric = GetCounterFactory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public ICounter<long> CreateCounterInt64(string name, string help, bool includeTimestamp = false)
    {
        var metric = CreateCounterInt64(name, help, ValueTuple.Create(), includeTimestamp);
        return metric.Unlabelled;
    }

    public IMetricFamily<ICounter<long>, ValueTuple<string>> CreateCounterInt64(string name, string help, string labelName, bool includeTimestamp = false)
    {
        return CreateCounterInt64(name, help, ValueTuple.Create(labelName), includeTimestamp);
    }

    public IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<ICounter<long>, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
            metric = CreateCounterInt64Internal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, params string[] labelNames)
    {
        return CreateCounterInt64(name, help, false, labelNames);
    }

    public IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        var metric = TryGetByName<IMetricFamily<ICounter<long>>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
            metric = GetCounterInt64Factory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }
}
