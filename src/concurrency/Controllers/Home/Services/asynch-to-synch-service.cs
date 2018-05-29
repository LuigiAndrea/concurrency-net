using System.Threading.Tasks;
using c = concurrency.Controllers.HomeController;

namespace concurrency.services
{
    public static class AsynchToSynchService
    {
        internal static async Task<string> GetTaskCompletionSourceValueAsync()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>(2);
            tcs.SetResult($"Returned from {nameof(c.GetTaskCompletionSource)}");
            string result = await tcs.Task;
            return result;
        }

        internal static async Task<string> GetResultValueAsync()
        {
            string result = await Task.FromResult($"Returned from {nameof(c.GetFromResult)}");
            return result;
        }
    }
}