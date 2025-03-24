using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public partial class MetricFactory : IMetricFactory
{
    public IGauge CreateGauge(string name, string help, bool includeTimestamp = false)
    {
        var metric = CreateGauge(name, help, ValueTuple.Create(), includeTimestamp);
        return metric.Unlabelled;
    }

    public IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<IGauge, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
            metric = CreateGaugeInternal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<IGauge> CreateGauge(string name, string help, params string[] labelNames)
    {
        return CreateGauge(name, help, false, labelNames);
    }

    public IMetricFamily<IGauge, ValueTuple<string>> CreateGauge(string name, string help, string labelName, bool includeTimestamp = false)
    {
        return CreateGauge(name, help, ValueTuple.Create(labelName), includeTimestamp);
    }

    public IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        var metric = TryGetByName<IMetricFamily<IGauge>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
            metric = GetGaugeFactory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IGauge<long> CreateGaugeInt64(string name, string help, bool includeTimestamp = false)
    {
        var metric = CreateGaugeInt64(name, help, ValueTuple.Create(), includeTimestamp);
        return metric.Unlabelled;
    }

    public IMetricFamily<IGauge<long>, ValueTuple<string>> CreateGaugeInt64(string name, string help, string labelName, bool includeTimestamp = false)
    {
        return CreateGaugeInt64(name, help, ValueTuple.Create(labelName), includeTimestamp);
    }

    public IMetricFamily<IGauge<long>, TLabels> CreateGaugeInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<IGauge<long>, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp);
            metric = CreateGaugeInt64Internal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, params string[] labelNames)
    {
        return CreateGaugeInt64(name, help, false, labelNames);
    }

    public IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        var metric = TryGetByName<IMetricFamily<IGauge<long>>>(name);
        if (metric == null)
        {
            var configuration = new MetricConfiguration(name, help, labelNames, includeTimestamp);
            metric = GetGaugeInt64Factory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

}
