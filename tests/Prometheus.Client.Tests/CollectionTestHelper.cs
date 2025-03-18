using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;
using Xunit;

namespace Prometheus.Client.Tests;

internal static class CollectionTestHelper
{
    public static async Task<string> CollectAsync(Action<IMetricFactory> metricsSetup)
    {
        var registry = new CollectorRegistry();
        var factory = new MetricFactory(registry);

        metricsSetup(factory);

        string formattedText;

        using (var stream = new MemoryStream())
        {
            using (var writer = new MetricsTextWriter(stream))
            {
                await registry.CollectToAsync(writer);

                await writer.CloseWriterAsync();
            }

            stream.Seek(0, SeekOrigin.Begin);

            using (var streamReader = new StreamReader(stream))
            {
                formattedText = await streamReader.ReadToEndAsync();
            }
        }

        return formattedText;
    }

    public static async Task TestCollectionAsync(Action<IMetricFactory> metricsSetup, string resourceName)
    {
        var formattedText = await CollectAsync(metricsSetup);

        Assert.Equal(GetFileContent(resourceName), formattedText);
    }

    private static string GetFileContent(string resourcePath)
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd().ToUnixLineEndings();
    }
}
