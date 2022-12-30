using System;

namespace Prometheus.Client;

public static class MetricFamilyExtensions
{
    /// <summary>
    ///     Extension method to workaround lack of single item tuple support
    /// </summary>
    public static TMetric WithLabels<TMetric>(this IMetricFamily<TMetric, ValueTuple<string>> metricFamily, string label)
        where TMetric : IMetric
    {
        return metricFamily.WithLabels(ValueTuple.Create(label));
    }
}
