namespace Concurrency.Models.ProcessTasksAsTheyComplete
{
    public class TaskAndTime
    {
        public TaskToProcess TaskA { get; set; } = new TaskToProcess() { TaskName = "Task A" };
        public TaskToProcess TaskB { get; set; } = new TaskToProcess() { TaskName = "Task B" };
        public TaskToProcess TaskC { get; set; } = new TaskToProcess() { TaskName = "Task C" };
    }

    public class TaskToProcess
    {
        public int SecondsToComplete { get; set; }
        public string TaskName { get; set; }
    }

    /// <summary>
    /// Object used from the controller to pass the process results to its view
    /// </summary>
    public class ResultTaskProcess
    {
        public string Loop { get; set; }
        public string AsTheyComplete { get; set; }
    }
}