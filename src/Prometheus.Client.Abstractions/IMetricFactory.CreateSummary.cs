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
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        IMetricFamily<ISummary> CreateSummary(string name, string help, params string[] labelNames);

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        IMetricFamily<ISummary> CreateSummary(string name, string help, bool includeTimestamp = false, params string[] labelNames);

        /// <summary>
        ///     Create Summary.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            IReadOnlyList<QuantileEpsilonPair> objectives,
            TimeSpan maxAge,
            int? ageBuckets,
            int? bufCap);

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        ISummary CreateSummary(
            string name,
            string help,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="labelName">Label name.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary, ValueTuple<string>> CreateSummary(
            string name,
            string help,
            string labelName,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary, TLabels> CreateSummary<TLabels>(
            string name,
            string help,
            TLabels labelNames,
            bool includeTimestamp = false,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null)
#if NET6_0_OR_GREATER
            where TLabels : struct, ITuple, IEquatable<TLabels>;
#else
            where TLabels : struct, IEquatable<TLabels>;
#endif

        /// <summary>
        ///     Create Summary
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="help">Help text.</param>
        /// <param name="includeTimestamp">Include unix timestamp for metric.</param>
        /// <param name="labelNames">Array of label names.</param>
        /// <param name="objectives">.</param>
        /// <param name="maxAge"></param>
        /// <param name="ageBuckets"></param>
        /// <param name="bufCap"></param>
        IMetricFamily<ISummary> CreateSummary(
            string name,
            string help,
            string[] labelNames,
            bool includeTimestamp,
            IReadOnlyList<QuantileEpsilonPair> objectives = null,
            TimeSpan? maxAge = null,
            int? ageBuckets = null,
            int? bufCap = null);
    }
}
