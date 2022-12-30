using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors;

public interface ICollector
{
    CollectorConfiguration Configuration { get; }

    IReadOnlyList<string> MetricNames { get; }

    void Collect(IMetricsWriter writer);
}
