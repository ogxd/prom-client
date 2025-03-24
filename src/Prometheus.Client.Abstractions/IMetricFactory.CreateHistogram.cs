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
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IHistogram CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IHistogram, ValueTuple<string>> CreateHistogram(string name, string help, string labelName, bool includeTimestamp = false, double[] buckets = null);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram, TLabels> CreateHistogram<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false, double[] buckets = null)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, double[] buckets = null, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Histogram.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="buckets">Buckets.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IHistogram> CreateHistogram(string name, string help, bool includeTimestamp = false, double[] buckets = null, params string[] labelNames);
    }
}
