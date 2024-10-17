using LongRunningApp.Application.Models;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;

namespace LongRunningApp.Application.Services;

public sealed class TextProcessingService(IOptions<AppLayerSettings> options) : ITextProcessingService
{
    private readonly AppLayerSettings _layerSettings = options.Value;

    public async IAsyncEnumerable<ITextProcessingResult> ProcessText(
        ITextProcessingRequest request,
        IProgress<int> progress,
        [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));
        ArgumentNullException.ThrowIfNull(nameof(progress));

        if (string.IsNullOrWhiteSpace(request.Text) || cancellation.IsCancellationRequested)
        {
            yield return TextProcessingResult.Empty;
            yield break;
        }

        var performedText = PerformedText(request.Text);
        var random = new Random();

        for (var i = 0; i < performedText.Length; i++)
        {
            if (cancellation.IsCancellationRequested)
            {
                yield return TextProcessingResult.Empty;
                yield break;
            }

            await Task.Delay(random.Next(0, _layerSettings.DelayEmulationMaxSec * 1000));
            var currentProgress = Math.Max(1, ((i + 1) * 100) / performedText.Length);
            progress.Report(currentProgress);
            yield return new TextProcessingResult()
            {
                Text = performedText[i].ToString(),
                ProgressPercentage = currentProgress
            };
        }
    }

    private static string PerformedText(string text)
    {
        var processedResult = text.GroupBy(x => x)
                          .Select(x => new { x.Key, Count = x.Count() })
                          .OrderBy(x => x.Key)
                          .ThenBy(x => x.Count)
                          .Select(x => x.Key.ToString() + x.Count.ToString())
                          .ToList();
        processedResult.Add("/");

        var plainTextBytes = Encoding.UTF8.GetBytes(text);
        processedResult.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(text)));

        return string.Join("", processedResult);
    }
}
