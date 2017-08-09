using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace concurrency.services
{
    public class CancelAsync
    {
        private CancellationTokenSource cts;
        private const int delayBeforeCancelTest = 4;
        private const int delayBeforeFinishTest = 7; 
        internal async void Start()
        {
            try
            {
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                using (cts = CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    cts.CancelAfter(TimeSpan.FromSeconds(delayBeforeCancelTest));
                    CancellationToken combinedToken = cts.Token;
                    int r = await TestAsync(combinedToken);
                    Debug.WriteLine(r);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Operation Cancelled");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // this.start.IsEnabled = true;
                // this.cancel.IsEnabled = false;
            }
        }

        internal void Cancel()
        {
            this.cts.Cancel();
        }

        public static async Task<int> TestAsync(CancellationToken ct)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayBeforeFinishTest), ct);
            return 122;
        }
    }
}
