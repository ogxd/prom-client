using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public partial class MetricFactory : IMetricFactory
{
    public IHistogram CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null)
    {
        var metric = CreateHistogram(name, help, ValueTuple.Create(), includeTimestamp, buckets);
        return metric.Unlabelled;
    }

    public IMetricFamily<IHistogram, ValueTuple<string>> CreateHistogram(string name, string help, string labelName, bool includeTimestamp = false, double[] buckets = null)
    {
        return CreateHistogram(name, help, ValueTuple.Create(labelName), includeTimestamp, buckets);
    }

    public IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, double[] buckets = null)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<IHistogram, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new HistogramConfiguration(name, help, LabelsHelper.ToArray(labelNames), buckets, includeTimestamp);
            metric = CreateHistogramInternal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<IHistogram> CreateHistogram(string name, string help, params string[] labelNames)
    {
        return CreateHistogram(name, help, false, null, labelNames);
    }

    public IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        return CreateHistogram(name, help, includeTimestamp, null, labelNames);
    }

    public IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets = null, params string[] labelNames)
    {
        return CreateHistogram(name, help, false, buckets, labelNames);
    }

    public IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null, params string[] labelNames)
    {
        var metric = TryGetByName<IMetricFamily<IHistogram>>(name);
        if (metric == null)
        {
            var configuration = new HistogramConfiguration(name, help, labelNames, buckets, includeTimestamp);
            metric = GetHistogramFactory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }
}
