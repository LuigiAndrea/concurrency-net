using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using concurrency.services;
using concurrency.Models.ProcessTasksAsTheyComplete;

namespace concurrency.Controllers
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

            setHeaders();
            setTaskResults();
            return View();
        }

        private void setHeaders()
        {
            ViewBag.ForLoop = "Tasks processed with a loop";
            ViewBag.InOrder = "Tasks processed as they complete";
        }

        private void setTaskResults()
        {
            ViewData["Loop"] = getValuesFromTasksSorted(TasksSortedAsTheyAreProcessed[(int)TaskProcessed.Loop]);
            ViewData["AsTheyComplete"] = getValuesFromTasksSorted(TasksSortedAsTheyAreProcessed[(int)TaskProcessed.InOrder]); ;
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