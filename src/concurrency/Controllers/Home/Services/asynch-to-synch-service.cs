using System.Threading.Tasks;
using c = concurrency.Controllers.HomeController;

namespace concurrency.services
{
    public static class AsynchToSynchService
    {
        internal static Task<string> GetValueAsync()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>(2);
            tcs.SetResult($"Returned from {nameof(c.GetTCS)}");
            return tcs.Task;
        }

        internal static Task<string> GetResultValueAsync()
        {
            return Task.FromResult($"Returned from {nameof(c.GetFromResult)}");
        }
    }
}