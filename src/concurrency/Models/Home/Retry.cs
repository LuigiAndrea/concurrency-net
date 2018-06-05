namespace Concurrency.Models.Home
{
    public class Retry
    {
        public int NumberOfRetry { get; set; }
        public int DelayForANewRetry { get; set; }
        public double TimeoutBeforeFail { get; set; }
    }
}
