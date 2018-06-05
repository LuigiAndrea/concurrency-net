using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concurrency.Models.ProcessTasksAsTheyComplete;

namespace Concurrency.Services.ProcessTasksAsTheyComplete
{
    public class ProcessTasksAsTheyCompleteSerivice
    {
        private static Task<TaskToProcess> taskA;
        private static Task<TaskToProcess> taskB;
        private static Task<TaskToProcess> taskC;
        private static Task<TaskToProcess>[] tasks;
        private static TaskAndTime taskAndTime;

        internal static async Task<Task<TaskToProcess>[][]> ProcessTasksAsync(TaskAndTime tat)
        {
            setTaskAndTimeField(tat);
            Task<TaskToProcess>[] loopTasks = await processTasksWithLoopAsync();
            Task<TaskToProcess>[] inOrderTasks = await processTasksAsTheyCompleteAsync();
            return new Task<TaskToProcess>[2][] { loopTasks, inOrderTasks };
        }

        private static async Task<Task<TaskToProcess>[]> processTasksWithLoopAsync()
        {
            TasksInitializer();
            Task<TaskToProcess>[] tasksResult = new Task<TaskToProcess>[3];

            for (int i = 0; i < tasks.Length; i++)
            {
                Task<TaskToProcess> task = tasks[i];
                TaskToProcess result = await task.ConfigureAwait(false);
                tasksResult[i] = Task.FromResult(result);
            }

            return tasksResult;
        }
        private static async Task<Task<TaskToProcess>[]> processTasksAsTheyCompleteAsync()
        {
            TasksInitializer();
            Task<TaskToProcess>[] tasksResult = new Task<TaskToProcess>[3];

            int i = 0;
            Task[] processingTasks = tasks.Select(async task =>
            {
                TaskToProcess result = await task.ConfigureAwait(false);
                tasksResult[i++] = Task.FromResult(result);
            }).ToArray();

            await Task.WhenAll(processingTasks).ConfigureAwait(false);

            return tasksResult;
        }

        private static void setTaskAndTimeField(TaskAndTime tat) => taskAndTime = tat;
        
        private static void TasksInitializer()
        {
            taskA = delayAndReturnValueAsync(taskAndTime.TaskA);
            taskB = delayAndReturnValueAsync(taskAndTime.TaskB);
            taskC = delayAndReturnValueAsync(taskAndTime.TaskC);
            tasks = new[] { taskA, taskB, taskC };
        }

        private static async Task<TaskToProcess> delayAndReturnValueAsync(TaskToProcess task)
        {
            await Task.Delay(TimeSpan.FromSeconds(task.SecondsToComplete)).ConfigureAwait(false);

            return new TaskToProcess()
            {
                SecondsToComplete = task.SecondsToComplete,
                TaskName = task.TaskName
            };
        }
    }
}
