using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Concurrency.Services.ProcessTasksAsTheyComplete;
using Concurrency.Models.ProcessTasksAsTheyComplete;

namespace Concurrency.Controllers
{
    public class ProcessTasksAsTheyCompleteController : Controller
    {
        private Task<TaskToProcess>[][] TasksSortedAsTheyAreProcessed;
        private enum TaskProcessed { Loop, InOrder };
        public IActionResult Tasks() => View();

        [HttpPost]
        public async Task<ViewResult> Tasks(TaskAndTime tat)
        {
            TasksSortedAsTheyAreProcessed = await ProcessTasksAsTheyCompleteSerivice.ProcessTasksAsync(tat);

            setTaskResults();
            return View();
        }

        private void setTaskResults()
        {
            ResultTaskProcess rtp = new ResultTaskProcess();
            rtp.Loop = getValuesFromTasksSorted(TasksSortedAsTheyAreProcessed[(int)TaskProcessed.Loop]);
            rtp.AsTheyComplete = getValuesFromTasksSorted(TasksSortedAsTheyAreProcessed[(int)TaskProcessed.InOrder]);
            ViewData["Result"] = rtp;
        }

        private string getValuesFromTasksSorted(Task<TaskToProcess>[] tasks)
        {
            String result = string.Empty;
            TaskToProcess task;
            for (int i = 0; i < tasks.Length; i++)
            {
                task = tasks[i].Result;
                result += $"{task.TaskName} ({task.SecondsToComplete}s) - ";
            }
            return result.Substring(0,result.Length-2);
        }
    }
}