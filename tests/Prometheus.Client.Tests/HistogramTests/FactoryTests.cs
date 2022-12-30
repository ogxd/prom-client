using System;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests
{
    public class FactoryTests
    {
        [Fact]
        public void ThrowOnNameConflict_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");

            Assert.Throws<InvalidOperationException>(() => factory.CreateHistogram("test_histogram", string.Empty, "label1", "testlabel"));
            Assert.Throws<InvalidOperationException>(() => factory.CreateHistogram("test_histogram", string.Empty, new[] { "label1" }));
            Assert.Throws<InvalidOperationException>(() => factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2", "label3"));
        }

        [Fact]
        public void ThrowOnNameConflict_Tuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));

            Assert.Throws<InvalidOperationException>(() => factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create("label1")));
            Assert.Throws<InvalidOperationException>(() => factory.CreateHistogram("test_histogram", string.Empty, ("label1", "testlabel")));
            Assert.Throws<InvalidOperationException>(() => factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2", "label3")));
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Strings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Tuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_StringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty, "label1", "label2");
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty, ("label1", "label2"));

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_Empty()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty);
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty);

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStrings()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty, Array.Empty<string>());

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyTuples()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create());
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create());

            Assert.Equal(counter1, counter2);
        }

        [Fact]
        public void SameLabelsProducesSameMetric_EmptyStringAndTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var counter1 = factory.CreateHistogram("test_histogram", string.Empty, Array.Empty<string>());
            var counter2 = factory.CreateHistogram("test_histogram", string.Empty, ValueTuple.Create());

            // Cannot compare metrics families, because of different contracts, should check if sample the same
            Assert.Equal(counter1.Unlabelled, counter2.Unlabelled);
        }

        [Theory]
        [InlineData("le")]
        [InlineData("le", "label")]
        [InlineData("label", "le")]
        public void ThrowOnReservedLabelNames_Strings(params string[] labels)
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateHistogram("test_Histogram", string.Empty, labels));
        }

        [Fact]
        public void ThrowOnReservedLabelNames_Tuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            Assert.Throws<ArgumentException>(() => factory.CreateHistogram("test_Histogram", string.Empty, ValueTuple.Create("le")));
            Assert.Throws<ArgumentException>(() => factory.CreateHistogram("test_Histogram", string.Empty, ("le", "label")));
        }

        [Fact]
        public void SingleLabel_ConvertToTuple()
        {
            var registry = new CollectorRegistry();
            var factory = new MetricFactory(registry);

            var gauge = factory.CreateHistogram("metricname", "help", "label");
            Assert.Equal(typeof(ValueTuple<string>), gauge.LabelNames.GetType());
        }
    }
}
