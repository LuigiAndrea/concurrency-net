using System;
using System.Threading;
using System.Threading.Tasks;
using static concurrency.services.RetryUtilities;

namespace concurrency.services
{
    internal class RetryService
    {
        int delayBetweenRetry;
        double timeoutFail;
        const string value = "Value downloaded";
        internal int NumberOfRetry { get; private set; }
        internal TimeSpan NextDelay { get; private set; }
  
        public RetryService(int retry, int delay, double timeout){
            NumberOfRetry = retry;
            delayBetweenRetry = delay;
            timeoutFail = timeout;
        }

        internal async Task<string> DownloadStringWithRetries()
        {
            CancellationToken token = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutFail)).Token;

            NextDelay = TimeSpan.FromSeconds(delayBetweenRetry);

            for (int i = 0; i != NumberOfRetry; ++i)
            {
                try
                {
                    return await GetValueAsync(delayBetweenRetry, value, token);
                }
                catch
                {

                }

                await Task.Delay(NextDelay);
                NextDelay = NextDelay + NextDelay;
            }
            // Try one last time, allowing the error to propogate.
            return await GetValueAsync(delayBetweenRetry, value, token);
        }
    }

    internal static class RetryUtilities
    {
        internal static async Task<T> GetValueAsync<T>(int val, T value, CancellationToken cancelation)
        {
            await Task.Delay(TimeSpan.FromSeconds(val), cancelation).ConfigureAwait(false);
            return value;
        }
    }
}
