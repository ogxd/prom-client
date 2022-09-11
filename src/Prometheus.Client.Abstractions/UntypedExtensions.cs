using System;
#if HasITuple
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public static class UntypedExtensions
    {
        public static void Set(this IUntyped untyped, double val, DateTimeOffset timestamp)
        {
            untyped.Set(val, timestamp.ToUnixTimeMilliseconds());
        }

        public static void Set(this IMetricFamily<IUntyped> metricFamily, double val)
        {
            metricFamily.Unlabelled.Set(val);
        }

        public static void Set(this IMetricFamily<IUntyped> metricFamily, double val, int timestamp)
        {
            metricFamily.Unlabelled.Set(val, timestamp);
        }

        public static void Set(this IMetricFamily<IUntyped> metricFamily, double val, DateTimeOffset timestamp)
        {
            metricFamily.Unlabelled.Set(val, timestamp);
        }

        public static void Set<TLabels>(this IMetricFamily<IUntyped, TLabels> metricFamily, double val)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(val);
        }

        public static void Set<TLabels>(this IMetricFamily<IUntyped, TLabels> metricFamily, double val, int timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(val, timestamp);
        }

        public static void Set<TLabels>(this IMetricFamily<IUntyped, TLabels> metricFamily, double val, DateTimeOffset timestamp)
#if HasITuple
            where TLabels : struct, ITuple, IEquatable<TLabels>
#else
        where TLabels : struct, IEquatable<TLabels>
#endif
        {
            metricFamily.Unlabelled.Set(val, timestamp);
        }

        public static IMetricFamily<IUntyped, ValueTuple<string>> CreateUntyped(this IMetricFactory factory, string name, string help, string labelName,
            bool includeTimestamp = false)
        {
            return factory.CreateUntyped(name, help, ValueTuple.Create(labelName), includeTimestamp);
        }
    }
}
