extern alias Their;
using System;
using BenchmarkDotNet.Attributes;

namespace Prometheus.Client.Benchmarks.Comparison.Summary;

public class SummarySampleBenchmarks : ComparisonBenchmarkBase
{
    private const int _opIterations = 100000;

    private ISummary _summary;
    private Their.Prometheus.ISummary _theirSummary;
    private double[] _dataset;

    [IterationSetup]
    public void Setup()
    {
        _summary = OurMetricFactory.CreateSummary("testSummary1", HelpText);
        _theirSummary = TheirMetricFactory.CreateSummary("testSummary1", HelpText);
        _dataset = new double[_opIterations];

        var rnd = new Random();
        for (int i = 0; i < _opIterations; i++)
        {
            _dataset[i] = rnd.NextDouble() * 100_000d;
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Observe")]
    public void Observe_Baseline()
    {
        for (var i = 0; i < _opIterations; i++)
            _theirSummary.Observe(_dataset[i]);
    }

    [Benchmark]
    [BenchmarkCategory("Observe")]
    public void Observe()
    {
        for (var i = 0; i < _opIterations; i++)
            _summary.Observe(_dataset[i]);
    }
}
