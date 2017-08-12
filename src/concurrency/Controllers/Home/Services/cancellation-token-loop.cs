using System;
using System.Threading;
using System.Threading.Tasks;

namespace concurrency.services
{
    public class CancellationTokenLoop
    {
        public async Task<string> startLoop(double timeout)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            var token = cts.Token;
            string result = string.Empty;

            try
            {
                result = await CancelableLoop(token);

            }
            catch (OperationCanceledException)
            {
                result = "Operation Cancelled";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public async Task<string> CancelableLoop(CancellationToken cancellationToken)
        {
            for (int i = 0; i != 100000; ++i)
            {
                //some work
                await Task.Delay(TimeSpan.FromMilliseconds(.2));
                cancellationToken.ThrowIfCancellationRequested();
            }

            return "completed";
        }
    }
}