using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static concurrency.services.AsynchToSynch;
using RetryService = concurrency.services.Retry;
using concurrency.Models;

namespace concurrency.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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
                ViewData["Result"] = $"Retried {(retry.numberOfRetry>1 ? retry.numberOfRetry + " times" : retry.numberOfRetry + " time")}: {ex.Message}";
            }
            return View("Index");
        }

        public IActionResult Retry()
        {
            return View();
        }

        public IActionResult CancelAsync(){
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
