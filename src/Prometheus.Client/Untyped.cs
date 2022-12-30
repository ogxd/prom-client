using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.MetricsWriter;

namespace Prometheus.Client;

/// <inheritdoc cref="IUntyped" />
public sealed class Untyped : MetricBase<MetricConfiguration>, IUntyped
{
    internal Untyped(MetricConfiguration configuration, IReadOnlyList<string> labels)
        : base(configuration, labels)
    {
    }

    private ThreadSafeDouble _value;

    public void Set(double val)
    {
        Set(val, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(double val, long? timestamp)
    {
        _value.Value = val;
        TrackObservation(timestamp);
    }

    public double Value => _value.Value;

    public void Reset()
    {
        _value.Value = default;
    }

    protected internal override void Collect(IMetricsWriter writer)
    {
        writer.WriteSample(Value, string.Empty, Configuration.LabelNames, LabelValues, Timestamp);
    }
}
