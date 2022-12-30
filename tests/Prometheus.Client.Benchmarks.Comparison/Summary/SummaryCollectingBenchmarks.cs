extern alias Their;
using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary;

public class SummaryCollectingBenchmarks : ComparisonBenchmarkBase
{
    private const int _metricsCount = 100;
    private const int _labelsCount = 5;
    private const int _variantsCount = 100;
    private const int _observationsCount = 100;

    public SummaryCollectingBenchmarks()
    {
        var labelNames = GenerateLabelNames(_labelsCount).ToArray();
        var labelVariants = GenerateLabelValues(_variantsCount, _labelsCount);
        var rnd = new Random();

        foreach (var metric in GenerateMetricNames(_metricsCount, 0))
        {
            var ourMetric = OurMetricFactory.CreateSummary(metric, HelpText, labelNames);
            var theirMetric = TheirMetricFactory.CreateSummary(metric, HelpText, labelNames);

            foreach (var labels in labelVariants)
            {
                for (var i = 0; i < _observationsCount; i++)
                {
                    var val = rnd.Next(10);
                    ourMetric.WithLabels(labels).Observe(val);
                    theirMetric.WithLabels(labels).Observe(val);
                }
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void Collecting_Baseline()
    {
        using var stream = Stream.Null;
        TheirCollectorRegistry.CollectAndExportAsTextAsync(stream).GetAwaiter().GetResult();
    }

    [Benchmark]
    public void Collecting()
    {
        using var stream = Stream.Null;
        ScrapeHandler.ProcessAsync(OurCollectorRegistry , stream).GetAwaiter().GetResult();
    }
}
