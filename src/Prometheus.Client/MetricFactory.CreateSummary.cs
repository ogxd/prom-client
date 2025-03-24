using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public partial class MetricFactory : IMetricFactory
{
    public IMetricFamily<ISummary> CreateSummary(string name, string help, params string[] labelNames)
    {
        return CreateSummary(name, help, false, labelNames);
    }

    public IMetricFamily<ISummary> CreateSummary(string name, string help, bool includeTimestamp = false, params string[] labelNames)
    {
        return CreateSummary(name, help, labelNames, includeTimestamp);
    }

    public IMetricFamily<ISummary> CreateSummary(
        string name,
        string help,
        string[] labelNames,
        IReadOnlyList<QuantileEpsilonPair> objectives,
        TimeSpan maxAge,
        int? ageBuckets,
        int? bufCap)
    {
        return CreateSummary(name, help, labelNames, false, objectives, maxAge, ageBuckets, bufCap);
    }

    public ISummary CreateSummary(
        string name,
        string help,
        bool includeTimestamp = false,
        IReadOnlyList<QuantileEpsilonPair> objectives = null,
        TimeSpan? maxAge = null,
        int? ageBuckets = null,
        int? bufCap = null)
    {
        var metric = CreateSummary(name, help, ValueTuple.Create(), includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
        return metric.Unlabelled;
    }

    public IMetricFamily<ISummary, ValueTuple<string>> CreateSummary(
        string name,
        string help,
        string labelName,
        bool includeTimestamp = false,
        IReadOnlyList<QuantileEpsilonPair> objectives = null,
        TimeSpan? maxAge = null,
        int? ageBuckets = null,
        int? bufCap = null)
    {
        return CreateSummary(name, help, ValueTuple.Create(labelName), includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
    }

    public IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
        string name,
        string help,
        TLabels labelNames,
        bool includeTimestamp = false,
        IReadOnlyList<QuantileEpsilonPair> objectives = null,
        TimeSpan? maxAge = null,
        int? ageBuckets = null,
        int? bufCap = null)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        var metric = TryGetByName<IMetricFamily<ISummary, TLabels>>(name);
        if (metric == null)
        {
            var configuration = new SummaryConfiguration(name, help, LabelsHelper.ToArray(labelNames), includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
            metric = CreateSummaryInternal<TLabels>(configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }

    public IMetricFamily<ISummary> CreateSummary(
        string name,
        string help,
        string[] labelNames,
        bool includeTimestamp,
        IReadOnlyList<QuantileEpsilonPair> objectives = null,
        TimeSpan? maxAge = null,
        int? ageBuckets = null,
        int? bufCap = null)
    {
        var metric = TryGetByName<IMetricFamily<ISummary>>(name);
        if (metric == null)
        {
            var configuration = new SummaryConfiguration(name, help, labelNames, includeTimestamp, objectives, maxAge, ageBuckets, bufCap);
            metric = GetSummaryFactory(labelNames?.Length ?? 0)(this, configuration);
        }
        else
        {
            ValidateLabelNames(metric, labelNames);
        }

        return metric;
    }
}
