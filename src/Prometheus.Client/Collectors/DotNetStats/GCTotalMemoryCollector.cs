using System;
using System.Collections.Generic;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client.Collectors.DotNetStats
{
    public class GCTotalMemoryCollector : ICollector
    {
        private const string _help = "Total known allocated memory in bytes";
        private readonly string _name;

        public GCTotalMemoryCollector()
            : this(string.Empty)
        {
        }

        public GCTotalMemoryCollector(string prefixName)
        {
            _name = prefixName + "dotnet_total_memory_bytes";
            Configuration = new CollectorConfiguration(nameof(GCTotalMemoryCollector));
            MetricNames = new[] { _name };
        }

        public CollectorConfiguration Configuration { get; }

        public IReadOnlyList<string> MetricNames { get; }

        public void Collect(IMetricsWriter writer)
        {
            writer.WriteMetricHeader(_name, MetricType.Gauge, _help);
            writer.WriteSample(GC.GetTotalMemory(false));
            writer.EndMetric();
        }
    }
}
