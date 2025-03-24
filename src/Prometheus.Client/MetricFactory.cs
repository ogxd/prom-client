using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public partial class MetricFactory : IMetricFactory
{
    private readonly ICollectorRegistry _registry;
    private readonly object _factoryProxyLock = new();
    private Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter>>[] _counterFactoryProxies;
    private Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter<long>>>[] _counterInt64FactoryProxies;
    private Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge>>[] _gaugeFactoryProxies;
    private Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge<long>>>[] _gaugeInt64FactoryProxies;
    private Func<MetricFactory, MetricConfiguration, IMetricFamily<IUntyped>>[] _untypedFactoryProxies;
    private Func<MetricFactory, HistogramConfiguration, IMetricFamily<IHistogram>>[] _histogramFactoryProxies;
    private Func<MetricFactory, SummaryConfiguration, IMetricFamily<ISummary>>[] _summaryFactoryProxies;

    public MetricFactory(ICollectorRegistry registry)
    {
        _registry = registry;
    }

    public void Release(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Value cannot be null or empty.", nameof(name));

        _registry.Remove(name);
    }

    public void Release<TMetric>(IMetricFamily<TMetric> metricFamily)
        where TMetric : IMetric
    {
        if (metricFamily == null)
            throw new ArgumentNullException(nameof(metricFamily));

        _registry.Remove(metricFamily.Name);
    }

    public void Release<TMetric, TLabels>(IMetricFamily<TMetric, TLabels> metricFamily)
        where TMetric : IMetric
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        if (metricFamily == null)
            throw new ArgumentNullException(nameof(metricFamily));

        _registry.Remove(metricFamily.Name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TCollector TryGetByName<TCollector>(string name)
    {
        if (_registry.TryGet(name, out var collector))
        {
            if (collector is TCollector metric)
                return metric;

            var prop = collector.GetType().GetProperty("LabelNames");
            if (prop != null)
            {
                var expectedLabels = prop.GetValue(collector);
                throw new InvalidOperationException($"Metric name ({name}). Must have same Type. Expected labels {expectedLabels}");
            }

            throw new InvalidOperationException($"Metric name ({name}). Must have same Type");
        }

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateLabelNames<TMetric, TLabels>(IMetricFamily<TMetric, TLabels> metric, TLabels actualNames)
        where TMetric : IMetric
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        if (LabelsHelper.GetHashCode(metric.LabelNames) != LabelsHelper.GetHashCode(actualNames))
        {
            throw new InvalidOperationException(
                $"Metric name ({metric.Name}). Expected labels {metric.LabelNames.ToString()}, but actual labels {actualNames.ToString()}");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateLabelNames<TMetric>(IMetricFamily<TMetric> metric, IReadOnlyList<string> actualNames)
        where TMetric : IMetric
    {
        if (metric.LabelNames == null && actualNames == null)
            return;

        var expectedNames = metric.LabelNames ?? Array.Empty<string>();
        actualNames ??= Array.Empty<string>();

        if (LabelsHelper.GetHashCode(expectedNames) != LabelsHelper.GetHashCode(actualNames))
        {
            throw new InvalidOperationException(
                $"Metric name ({metric.Name}). Expected labels ({string.Join(", ", expectedNames)}), but actual labels ({string.Join(", ", actualNames)})");
        }
    }

    internal MetricFamily<ICounter, Counter, TLabels, MetricConfiguration> CreateCounterInternal<TLabels>(MetricConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<ICounter, Counter, TLabels, MetricConfiguration>(
                config,
                MetricType.Counter,
                (cfg, labelNames) => new Counter(cfg, labelNames)));
    }

    internal MetricFamily<ICounter<long>, CounterInt64, TLabels, MetricConfiguration> CreateCounterInt64Internal<TLabels>(MetricConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<ICounter<long>, CounterInt64, TLabels, MetricConfiguration>(
                config,
                MetricType.Counter,
                (cfg, labelNames) => new CounterInt64(cfg, labelNames)));
    }

    internal MetricFamily<IGauge, Gauge, TLabels, MetricConfiguration> CreateGaugeInternal<TLabels>(MetricConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<IGauge, Gauge, TLabels, MetricConfiguration>(
                config,
                MetricType.Gauge,
                (cfg, labelNames) => new Gauge(cfg, labelNames)));
    }

    internal MetricFamily<IGauge<long>, GaugeInt64, TLabels, MetricConfiguration> CreateGaugeInt64Internal<TLabels>(MetricConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<IGauge<long>, GaugeInt64, TLabels, MetricConfiguration>(
                config,
                MetricType.Gauge,
                (cfg, labelNames) => new GaugeInt64(cfg, labelNames)));
    }

    internal MetricFamily<IHistogram, Histogram, TLabels, HistogramConfiguration> CreateHistogramInternal<TLabels>(HistogramConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<IHistogram, Histogram, TLabels, HistogramConfiguration>(
                config,
                MetricType.Histogram,
                (cfg, labelNames) => new Histogram(cfg, labelNames)));
    }

    internal MetricFamily<IUntyped, Untyped, TLabels, MetricConfiguration> CreateUntypedInternal<TLabels>(MetricConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<IUntyped, Untyped, TLabels, MetricConfiguration>(
                config,
                MetricType.Untyped,
                (cfg, labelNames) => new Untyped(cfg, labelNames)));
    }

    internal MetricFamily<ISummary, Summary, TLabels, SummaryConfiguration> CreateSummaryInternal<TLabels>(SummaryConfiguration configuration)
#if NET6_0_OR_GREATER
        where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
    {
        return _registry.GetOrAdd(configuration,
            config => new MetricFamily<ISummary, Summary, TLabels, SummaryConfiguration>(
                config,
                MetricType.Summary,
                (cfg, labelNames) => new Summary(cfg, labelNames)));
    }

    internal Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter>> GetCounterFactory(int labelNamesLen)
    {
        return GetFactory(ref _counterFactoryProxies, nameof(CreateCounterInternal), labelNamesLen);
    }

    internal Func<MetricFactory, MetricConfiguration, IMetricFamily<ICounter<long>>> GetCounterInt64Factory(int labelNamesLen)
    {
        return GetFactory(ref _counterInt64FactoryProxies, nameof(CreateCounterInt64Internal), labelNamesLen);
    }

    internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge>> GetGaugeFactory(int labelNamesLen)
    {
        return GetFactory(ref _gaugeFactoryProxies, nameof(CreateGaugeInternal), labelNamesLen);
    }

    internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IGauge<long>>> GetGaugeInt64Factory(int labelNamesLen)
    {
        return GetFactory(ref _gaugeInt64FactoryProxies, nameof(CreateGaugeInt64Internal), labelNamesLen);
    }

    internal Func<MetricFactory, MetricConfiguration, IMetricFamily<IUntyped>> GetUntypedFactory(int labelNamesLen)
    {
        return GetFactory(ref _untypedFactoryProxies, nameof(CreateUntypedInternal), labelNamesLen);
    }

    internal Func<MetricFactory, HistogramConfiguration, IMetricFamily<IHistogram>> GetHistogramFactory(int labelNamesLen)
    {
        return GetFactory(ref _histogramFactoryProxies, nameof(CreateHistogramInternal), labelNamesLen);
    }

    internal Func<MetricFactory, SummaryConfiguration, IMetricFamily<ISummary>> GetSummaryFactory(int labelNamesLen)
    {
        return GetFactory(ref _summaryFactoryProxies, nameof(CreateSummaryInternal), labelNamesLen);
    }

    private Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>> GetFactory<TConfiguration, TMetric>(
        ref Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>[] cache, string targetMethodName, int labelNamesLen)
        where TConfiguration : MetricConfiguration
        where TMetric : IMetric
    {
        if (cache?.GetUpperBound(0) > labelNamesLen)
            return cache[labelNamesLen];

        lock (_factoryProxyLock)
        {
            if (cache?.GetUpperBound(0) > labelNamesLen)
                return cache[labelNamesLen];

            var tmp = new Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>[labelNamesLen + 1];
            if (cache != null)
                Array.Copy(cache, tmp, cache.Length);

            var configurationParameter = Expression.Parameter(typeof(TConfiguration), "configuration");
            var factoryParameter = Expression.Parameter(typeof(MetricFactory), "factory");
            for (var i = cache?.Length ?? 0; i <= labelNamesLen; i++)
            {
                var labelNamesTupleType = LabelsHelper.MakeValueTupleType(i);

                var targetMethodCall = Expression.Call(
                    factoryParameter,
                    targetMethodName,
                    new[] { labelNamesTupleType },
                    configurationParameter);

                tmp[i] = Expression.Lambda<Func<MetricFactory, TConfiguration, IMetricFamily<TMetric>>>(targetMethodCall, factoryParameter, configurationParameter).Compile();
            }

            cache = tmp;
        }

        return cache[labelNamesLen];
    }
}
