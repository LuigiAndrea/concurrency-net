using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace concurrency.services
{
    public class CancelAsyncService
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        private const int delayBeforeCancelTest = 4;
        private const int delayBeforeFinishTest = 7; 
        internal async Task<string> Start()
        {
            string result=string.Empty;
            try
            {
                CancellationToken token = cts.Token;
                using (cts = CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    cts.CancelAfter(TimeSpan.FromSeconds(delayBeforeCancelTest));
                    CancellationToken combinedToken = cts.Token;
                    result = await TestAsync(combinedToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                result = "Operation Cancelled by timeout";
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        internal string Cancel()
        {
            this.cts.Cancel();
            return "Operation cancel by user";
        }

        public static async Task<string> TestAsync(CancellationToken ct)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayBeforeFinishTest), ct);
            return $"Task Completed after {delayBeforeFinishTest}";
        }
    }
}
