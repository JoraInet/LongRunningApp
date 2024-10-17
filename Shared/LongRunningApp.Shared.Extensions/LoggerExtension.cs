using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongRunningApp.Shared.Extensions;

public static class LoggerExtension
{
    public static bool LogErrorIfNullOrWhiteSpace(this ILogger logger, string value, string valueName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            logger.LogError($"'{valueName}' cannot be null or whitespace.");
            return true;
        }
        return false;
    }
}
