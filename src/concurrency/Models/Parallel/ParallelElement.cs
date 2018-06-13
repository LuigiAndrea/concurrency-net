namespace Concurrency.Models.Parallel
{
    /// <summary>
    /// Used by SumInParell view to post data
    /// </summary>
    public class ParallelElement
    {
        public long NumberOfElements { get; set; } = 9000000L;
        
    }

    /// <summary>
    /// Used by Parallel.service to return the time for the three operations performed to ParallelController
    /// </summary>
    public class ParallelTime {
        public double ParallelSumRegular { get; set; }
        public double ParallelSumWithAsParallel { get; set; }
        public double ParallelSumWithAggregate { get; set; }
    }
}