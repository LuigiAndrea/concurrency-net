using System;
using System.Threading.Tasks;

namespace concurrency.services
{
    public static class AggregateExceptionService
    {
        internal enum CatchExc { All, One };
        internal static async Task<string> GetExceptions(CatchExc c)
        {
            string result = string.Empty;
            if (c == CatchExc.All)
            {
                int i = 0;
                try
                {
                    await ObserveAllExceptionsAsync();

                }
                catch (AggregateException e)
                {
                    result = $"{nameof(ObserveAllExceptionsAsync)} List of Exceptions: ";

                    e.Handle(x =>
                    {
                        result += $"{++i}: {x.Message}";
                        return true;
                    });
                }
            }
            else
            {
                try
                {
                    await ObserveOneExceptionAsync();
                }
                catch (NotImplementedException e)
                {
                    result = $"{nameof(ObserveOneExceptionAsync)} {e.Message}";
                }
            }

            return result;
        }
        private static async Task ObserveOneExceptionAsync()
        {
            var task1 = ThrowNotImplementedExceptionAsync();
            var task2 = ThrowInvalidOperationExceptionAsync();

            try
            {
                await Task.WhenAll(task1, task2);
            }
            catch
            {
                throw;
            }
        }

        private static async Task ObserveAllExceptionsAsync()
        {
            var task1 = ThrowNotImplementedExceptionAsync();
            var task2 = ThrowInvalidOperationExceptionAsync();
            Task allTasks = Task.WhenAll(task1, task2);
            try
            {
                await allTasks;
            }
            catch
            {
                AggregateException allExceptions = allTasks.Exception;

                throw allExceptions;
            }
        }
        private static async Task ThrowNotImplementedExceptionAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            throw new NotImplementedException();
        }
        private static async Task ThrowInvalidOperationExceptionAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            throw new InvalidOperationException();
        }
    }
}
