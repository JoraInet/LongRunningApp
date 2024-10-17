using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongRunningApp.Application.Managers;
internal sealed class ProcessingTask : IProcessingTask
{
    public CancellationTokenSource CancellationTokenSource { get; set; }
    public IProgress<int> ProgressPercentage { get; set; }
    public Task TaskInProgress { get; set; }
}
