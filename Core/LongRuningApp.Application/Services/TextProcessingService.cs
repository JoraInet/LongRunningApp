using LongRunningApp.Application.Models;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;

namespace LongRunningApp.Application.Services;

public sealed class TextProcessingService(IOptions<AppLayerSettings> options) : ITextProcessingService
{
    private readonly AppLayerSettings _layerSettings = options.Value;

    public async IAsyncEnumerable<string> ProcessText(string text, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            yield return string.Empty;
            yield break;
        }

        if (cancellation.IsCancellationRequested)
        {
            yield return string.Empty;
            yield break;
        }

        var performedText = PerformedText(text);
        var random = new Random();
        foreach (var p in performedText)
        {
            if (cancellation.IsCancellationRequested)
            {
                yield return string.Empty;
                yield break;
            }

            await Task.Delay(random.Next(0, _layerSettings.DelayEmulationMaxSec * 1000));

            yield return p.ToString();
        }
    }

    private static string PerformedText(string text)
    {
        var processedResult = text.GroupBy(x => x)
                          .Select(x => new { Key = x.Key.ToString(), Count = x.Count() })
                          .OrderBy(x => x.Count)
                          .ThenBy(x => x.Key)
                          .Select(x => x.Key.ToString() + x.Count.ToString())
                          .ToList();
        processedResult.Add("/");

        var plainTextBytes = Encoding.UTF8.GetBytes(text);
        processedResult.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(text)));

        return string.Join("", processedResult);
    }
}
