using System;
using System.Threading;
using System.Threading.Tasks;
using c = concurrency.Controllers.HomeController;

namespace concurrency.services
{
    public class Retry
    {
        const int delay = 1;
        const double timeoutFail = 0.5;
        const int defaultNumberOfRetry = 3;
        const string str = "String downloaded";
        public int numberOfRetry{ get; private set;}

        public Retry(int retry) => numberOfRetry = (retry >= 1 && retry<=10) ? retry : defaultNumberOfRetry;

        private async Task<string> getStringAsync(int val, string str, CancellationToken cancelation)
        {
            await Task.Delay(TimeSpan.FromSeconds(val), cancelation).ConfigureAwait(false);
            return str;
        }

        internal async Task<string> DownloadStringWithRetries(bool fail)
        {
            CancellationToken token = (fail) ? new CancellationTokenSource(TimeSpan.FromSeconds(timeoutFail)).Token
                                             : CancellationToken.None;

            var nextDelay = TimeSpan.FromSeconds(1);

            for (int i = 0; i != numberOfRetry; ++i)
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
