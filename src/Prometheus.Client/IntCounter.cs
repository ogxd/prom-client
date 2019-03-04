using System;
using Prometheus.Client.Abstractions;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Prometheus.Client.Tools;

namespace Prometheus.Client
{
    public class IntCounter : Collector<IntCounter.LabelledCounter, MetricConfiguration>, ICounter<long>
    {
        internal IntCounter(MetricConfiguration configuration)
            : base(configuration)
        {
        }

        protected override MetricType Type => MetricType.Counter;

        public void Inc()
        {
            Unlabelled.Inc();
        }

        public void Inc(long increment)
        {
            Unlabelled.Inc(increment);
        }

        public long Value => Unlabelled.Value;

        public void Reset()
        {
            Unlabelled.ResetValue();
            foreach (var labelledMetric in LabelledMetrics)
                labelledMetric.Value.ResetValue();
        }

        public class LabelledCounter : Labelled<MetricConfiguration>, ICounter<long>
        {
            private ThreadSafeLong _value;

            public void Inc()
            {
                Inc(1);
            }

            public void Inc(long increment)
            {
                if (increment < 0)
                    throw new ArgumentOutOfRangeException(nameof(increment), "Counter cannot go down");

                _value.Add(increment);
                TimestampIfRequired();
            }

            public long Value => _value.Value;

            protected internal override void Collect(IMetricsWriter writer)
            {
                writer.WriteSample(Value, string.Empty, Labels, Timestamp);
            }

            internal void ResetValue()
            {
                _value.Value = 0;
            }
        }
    }
}
