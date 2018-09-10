using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concurrency.Models.PerfornabceImmutableList;

namespace Concurrency.Services.PerformanceImmutableList
{
    public class PerformanceImmutableListSerivice
    {
        const int nanosec = 1000000;
        public static async Task<PerformanceResult> RunTest(int numberOfElements){

            ImmutableList<int>.Builder immutableList = await populateImmmutableList(numberOfElements);
            PerformanceResult pr = new PerformanceResult();
            pr.ForeachTime = await calculateForeachTime(immutableList);
            pr.ForTime = await calculateForTime(immutableList);

            return pr;
        }

        private static Task<ImmutableList<int>.Builder> populateImmmutableList(int size)
        {
            ImmutableList<int>.Builder immutableList = ImmutableList.CreateBuilder<int>();

            for (int i = 0; i < size; i++)
            {
                immutableList.Add(i);
            }

            return Task.FromResult(immutableList);
        }

        private static Task<TimeSpan> calculateForeachTime(ImmutableList<int>.Builder immutableList)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            executeForeachWork(immutableList);
            s.Stop();

            return Task.FromResult(new TimeSpan(s.Elapsed.Days, s.Elapsed.Hours, s.Elapsed.Minutes, s.Elapsed.Seconds, s.Elapsed.Milliseconds));
        }

        private static Task<TimeSpan> calculateForTime(ImmutableList<int>.Builder immutableList)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            executeForWork(immutableList);
            s.Stop();

            return Task.FromResult(new TimeSpan(s.Elapsed.Days, s.Elapsed.Hours, s.Elapsed.Minutes, s.Elapsed.Seconds, s.Elapsed.Milliseconds));
        }

        private static void executeForeachWork(ImmutableList<int>.Builder immutableList)
        {
            int result;
            foreach (var item in immutableList)
            {
                result = item % 2;
            }
        }

        private static void executeForWork(ImmutableList<int>.Builder immutableList)
        {
            int result;
            for (int i = 0; i < immutableList.Count; i++)
            {
                result = immutableList[i] % 2;
            }
        }


    }
}
