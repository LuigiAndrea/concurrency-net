using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using static Concurrency.Services.Home.AsynchToSynchService;
using static Concurrency.Services.Home.AggregateExceptionService;
using Concurrency.Services.Home;
using Concurrency.Models.Home;

namespace Concurrency.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public ViewResult GetFromResult()
        {
            ViewData["Result"] = GetResultValueAsync().Result;
            return View("Index");
        }

        public ViewResult GetTaskCompletionSource()
        {
            ViewData["Result"] = GetTaskCompletionSourceValueAsync().Result;
            return View("Index");
        }

        public IActionResult Retry() => View();

        [HttpPost]
        public async Task<ViewResult> DownloadWithRetry(Retry r)
        {
            RetryService retry = new RetryService(r.NumberOfRetry, r.DelayForANewRetry, r.TimeoutBeforeFail);

            try
            {
                ViewData["Result"] = await retry.DownloadStringWithRetries();
            }
            catch (Exception ex)
            {
                ViewData["Result"] =
                    $"Retried {(retry.NumberOfRetry > 1 ? retry.NumberOfRetry + " times" : retry.NumberOfRetry + " time")}" +
                    $" with a total delay between calls of {retry.NextDelay.Subtract(TimeSpan.FromSeconds(r.DelayForANewRetry))}. {ex.Message}";
            }
            return View("Index");
        }

        public IActionResult CancelAsync() => View();

        [HttpPost]
        public async Task<ViewResult> StartAsyncCode(CancelAsync ca)
        {
            CancelAsyncService c = new CancelAsyncService();
            ViewData["Result"] = await c.Start(ca.delayCompleteTask, ca.delayCancelTask);
            return View("Index");
        }
        public ViewResult CancelAsyncCode()
        {
            CancelAsyncService c = new CancelAsyncService();
            ViewData["Result"] = c.Cancel();
            return View("Index");
        }

        public IActionResult CancellationTokenLoop() => View();

        [HttpPost]
        public async Task<ViewResult> CancellationTokenLoop(CancellationLoop cancLoop)
        {
            CancellationTokenLoop ctl = new CancellationTokenLoop();
            ViewData["Result"] = await ctl.StartLoop(cancLoop.TimeoutCancellationToken,cancLoop.NumberOfIterationToPerformLoop);
            return View("Index");
        }

        public IActionResult AggregationException() => View();

        [HttpPost]
        public async Task<ViewResult> AggregationException(AggregationExceptions ae)
        {
            ViewData["Result"] = await GetExceptions(ae.CatchException.Equals("All") 
                                                            ? CatchExc.All : CatchExc.One);
            return View("Index");
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Error()
        {
            return View();
        }
    }
}
