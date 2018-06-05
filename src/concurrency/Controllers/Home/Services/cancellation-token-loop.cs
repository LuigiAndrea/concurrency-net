using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency.Services.Home
{
    public class CancellationTokenLoop
    {
        internal static long NumberOfIterationLoop {get; private set;}
        
        public async Task<string> StartLoop(double timeout,long numberOfIteration)
        {
            string result = string.Empty;       
            
            try
            { 
                if(numberOfIteration.Equals(0)) //number bigger than long.MaxValue
                    throw new OperationCanceledException();

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
                var token = cts.Token;
                NumberOfIterationLoop = numberOfIteration;
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