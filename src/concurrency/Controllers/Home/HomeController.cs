using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using static concurrency.services.AsynchToSynchService;
using concurrency.services;
using concurrency.Models;

namespace concurrency.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public ViewResult GetTCS()
        {
            ViewData["Result"] = GetValueAsync().Result;
            return View("Index");
        }

        public ViewResult GetFromResult()
        {
            ViewData["Result"] = GetResultValueAsync().Result;
            return View("Index");
        }

        public IActionResult Retry() => View();

        [HttpPost]
        public async Task<ViewResult> DownloadWithRetry(Retry r)
        {
            RetryService retry = new RetryService(r.numberOfRetry);

            try
            {
                ViewData["Result"] = await retry.DownloadStringWithRetries(r.fail);
            }
            catch (Exception ex)
            {
                ViewData["Result"] = $"Retried {(retry.numberOfRetry > 1 ? retry.numberOfRetry + " times" : retry.numberOfRetry + " time")} with a total delay between calls of {retry.nextDelay} : {ex.Message}";
            }
            return View("Index");
        }

        public IActionResult CancelAsync()
        {
            return View();
        }

        public async Task<ViewResult> StartAsyncCode(CancelAsync ca)
        {          
            CancelAsyncService c = new CancelAsyncService();
            ViewData["Result"] = await c.Start(ca.delayCompleteTask,ca.delayCancelTask);
            return View("Index");
        }
        public ViewResult CancelAsyncCode()
        {
            CancelAsyncService c = new CancelAsyncService();
            ViewData["Result"] = c.Cancel();
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
