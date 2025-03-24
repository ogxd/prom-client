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
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IGauge CreateGauge(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge, TLabels> CreateGauge<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IGauge, ValueTuple<string>> CreateGauge(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge> CreateGauge(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IGauge<long> CreateGaugeInt64(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IGauge<long>, ValueTuple<string>> CreateGaugeInt64(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>, TLabels> CreateGaugeInt64<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create int-based Gauge.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IGauge<long>> CreateGaugeInt64(string name, string help, bool includeTimestamp = false, params string[] labelNames);
    }
}
