using System;

namespace Concurrency.Models.PerfornabceImmutableList
{
    /// <summary>
    /// Used by the view to post data
    /// </summary>
    public class ImmutableListTest
    {
        public int NumberOfElements { get; set; }
        
    }

    /// <summary>
    /// Used by the service to return the result of the test
    /// </summary>
    public class PerformanceResult {
        public TimeSpan ForTime { get; set; }
        public TimeSpan ForeachTime { get; set; }
    }
}