using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongRunningApp.Application.Managers;
internal interface IProcessingTask
{
    CancellationTokenSource CancellationTokenSource { get; set; }
    IProgress<int> ProgressPercentage { get; set; }
    Task TaskInProgress { get; set; }
}
