using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency.Services.Home
{
    public class CancelAsyncService
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        private int delayBeforeFinishTest, delayBeforeCancelTest;
        internal async Task<string> Start(int completeTask, int timeoutTask)
        {
            delayBeforeFinishTest = completeTask;
            delayBeforeCancelTest = timeoutTask;

            string result = string.Empty;
            try
            {
                result = await handleOperation();

            }
            catch (OperationCanceledException)
            {
                result = $"Operation Cancelled by timeout after {delayBeforeCancelTest} {(delayBeforeCancelTest > 1 ? "seconds" : "second")}";
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private async Task<string> handleOperation()
        {
            CancellationToken token = cts.Token;
            using (cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(TimeSpan.FromSeconds(delayBeforeCancelTest));
                CancellationToken combinedToken = cts.Token;
                return await executeOperation(combinedToken).ConfigureAwait(false);
            }
        }
        internal string Cancel()
        {
            this.cts.Cancel();
            return "Operation cancel by user";
        }

        private async Task<string> executeOperation(CancellationToken ct)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayBeforeFinishTest), ct).ConfigureAwait(false);
            return $"Task Completed after {delayBeforeFinishTest} {(delayBeforeFinishTest > 1 ? "seconds" : "second")}";
        }
    }
}
