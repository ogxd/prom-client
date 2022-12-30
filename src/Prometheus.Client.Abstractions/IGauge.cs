namespace Prometheus.Client;

/// <summary>
///     Gauge metric type
///     <remarks>
///         https://prometheus.io/docs/concepts/metric_types/#gauge
///     </remarks>
/// </summary>
public interface IGauge<T> : IMetric<T>
    where T : struct
{
    void Inc();

    void Inc(T increment);

    void Inc(T increment, long? timestamp);

    void IncTo(T value);

    void IncTo(T value, long? timestamp);

    void Set(T val);

    void Set(T val, long? timestamp);

    void Dec();

    void Dec(T decrement);

    void Dec(T decrement, long? timestamp);

    void DecTo(T value);

    void DecTo(T value, long? timestamp);
}

public interface IGauge : IGauge<double>
{
}
