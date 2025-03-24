using System;
using System.Collections.Generic;

#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Prometheus.Client
{
    public partial interface IMetricFactory
    {
        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        ICounter CreateCounter(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<ICounter, ValueTuple<string>> CreateCounter(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        /// <param name="timeToLive">Remove metric if not incremented for the given duration</param>
        IMetricFamily<ICounter, TLabels> CreateCounter<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, TimeSpan timeToLive = default)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter> CreateCounter(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create  Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter> CreateCounter(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        ICounter<long> CreateCounterInt64(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<ICounter<long>, ValueTuple<string>> CreateCounterInt64(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>, TLabels> CreateCounterInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create int-based Counter.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<ICounter<long>> CreateCounterInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames);
    }
}
