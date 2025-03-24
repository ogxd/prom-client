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
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IUntyped CreateUntyped(string name, string help, bool includeTimestamp = false);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        IMetricFamily<IUntyped, ValueTuple<string>> CreateUntyped(string name, string help, string labelName, bool includeTimestamp = false);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped, TLabels> CreateUntyped<TLabels>(string name, string help, TLabels labelNames, bool includeTimestamp = false)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped> CreateUntyped(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Untyped.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Label names</param>
        IMetricFamily<IUntyped> CreateUntyped(string name, string help, bool includeTimestamp = false, params string[] labelNames);
    }
}
