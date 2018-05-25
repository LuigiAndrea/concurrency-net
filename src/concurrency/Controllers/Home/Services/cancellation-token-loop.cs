using System;
using System.Threading;
using System.Threading.Tasks;

namespace concurrency.services
{
    public class CancellationTokenLoop
    {
        internal static long NumberOfIterationLoop {get; private set;} = 100000;
        
        public async Task<string> StartLoop(double timeout,long numberOfIteration)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            var token = cts.Token;
            NumberOfIterationLoop = numberOfIteration;
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

        private async Task<string> CancelableLoop(CancellationToken cancellationToken)
        {
            for (int i = 0; i != NumberOfIterationLoop; ++i)
            {
                //some work
                await Task.Delay(TimeSpan.FromMilliseconds(.2));
                cancellationToken.ThrowIfCancellationRequested();
            }

            return "completed";
        }
    }
}