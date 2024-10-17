using Microsoft.Extensions.Logging;
using Moq;

namespace LongRunningApp.Application.UnitTests;
public static class MockExtensions
{
    public static bool VerifyLoggerHasExactMessage(this Mock mock, LogLevel logLevel, string message) 
        => mock.Invocations.Any(x => x.Method.Name == nameof(ILogger.Log) 
                                     && x.Arguments.OfType<LogLevel>()
                                                   .Any(ll => ll == logLevel) 
                                     && x.Arguments.OfType<IReadOnlyList<KeyValuePair<string, object?>>>()
                                                   .Any(kv => kv.Any(m => m.Value?.ToString() == message)));

    public static bool VerifyLoggerContainsMessage(this Mock mock, LogLevel logLevel, string message)
        => mock.Invocations.Any(x => x.Method.Name == nameof(ILogger.Log)
                                     && x.Arguments.OfType<LogLevel>()
                                                   .Any(ll => ll == logLevel)
                                     && x.Arguments.OfType<IReadOnlyList<KeyValuePair<string, object?>>>()
                                                   .Any(kv => kv.Any(m => m.Value?.ToString()?.Contains(message) == true )));
}
