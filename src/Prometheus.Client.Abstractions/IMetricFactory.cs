using System;
using System.Collections.Generic;

#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public partial interface IMetricFactory
    {
        void Release(string name);

        void Release<TMetric>(IMetricFamily<TMetric> metricFamily)
            where TMetric : IMetric;

        void Release<TMetric, TLabels>(IMetricFamily<TMetric, TLabels> metricFamily)
            where TMetric : IMetric
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif
    }
}
