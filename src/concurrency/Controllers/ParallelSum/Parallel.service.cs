using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concurrency.Models.Parallel;

namespace Concurrency.Services.Parallel
{
    public class ParallelSerivice
    {
        static List<long> randomNumbers = new List<long>();
       
        public async static Task<ParallelTime> SumElementsList(long numberOfElements){
            addRandomValuesToList(numberOfElements);

            ParallelTime pt = new ParallelTime();
            pt.ParallelSumRegular = await parallelSumRegular().ConfigureAwait(false);
            pt.ParallelSumWithAsParallel = await parallelSumWithAsParallel().ConfigureAwait(false);
            pt.ParallelSumWithAggregate = await parallelSumWithAggregate().ConfigureAwait(false);

            return pt;
        }

        private async static Task<double> parallelSumRegular()
        {
            Stopwatch s = new Stopwatch();
            s.Start();  
            long r = 0;
            foreach (long e in randomNumbers)
                r += e;
            s.Stop();

            return await Task.FromResult(s.Elapsed.TotalMilliseconds);      
        }

        private async static Task<double> parallelSumWithAsParallel(){
            Stopwatch s = new Stopwatch();
            s.Start();  
            randomNumbers.AsParallel().Sum();
            s.Stop();
            return await Task.FromResult(s.Elapsed.TotalMilliseconds);        
        }

        private async static Task<double> parallelSumWithAggregate()
        {
            Stopwatch s = new Stopwatch();
            s.Start();  
            randomNumbers.AsParallel().Aggregate(
                0, (long n, long i) => n + i
                );
            s.Stop();
            return await Task.FromResult(s.Elapsed.TotalMilliseconds);         
        }

        private static void addRandomValuesToList(long numberOfElements)
        {
            randomNumbers = new List<long>();
            Random r = new Random(1000);
            for (long i = 0; i < numberOfElements; i++)
            {   
                randomNumbers.Add(r.Next());
            }
        }
    }
}
