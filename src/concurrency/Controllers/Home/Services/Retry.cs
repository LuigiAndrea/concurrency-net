using System;
using System.Threading;
using System.Threading.Tasks;
using c = concurrency.Controllers.HomeController;

namespace concurrency.services
{
    public static class Retry
    {
        const int delay = 1;
        const double timeoutFail = 0.5;

        private static async Task<string> getStringAsync(int val, string str, CancellationToken cancelation)
        {
            await Task.Delay(TimeSpan.FromSeconds(val), cancelation).ConfigureAwait(false);
            return str;
        }

        internal static async Task<string> DownloadStringWithRetries(bool fail)
        {
            CancellationToken token = (fail) ? new CancellationTokenSource(TimeSpan.FromSeconds(timeoutFail)).Token
                                             : CancellationToken.None;
            const string str = "String downloaded";
            // Retry after 1 second, then after 2 seconds, then 4.
            var nextDelay = TimeSpan.FromSeconds(1);
            for (int i = 0; i != 3; ++i)
            {
                try
                {
                    return await getStringAsync(delay, str, token);
                }
                catch
                {

                }

                await Task.Delay(nextDelay);
                nextDelay = nextDelay + nextDelay;
            }
            // Try one last time, allowing the error to propogate.
            return await getStringAsync(delay, str, token);
        }
    }
}
