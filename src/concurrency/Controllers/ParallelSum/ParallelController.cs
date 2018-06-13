using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Concurrency.Services.Parallel;
using Concurrency.Models.Parallel;

namespace Concurrency.Controllers
{
    public class ParallelController : Controller
    {
        public IActionResult SumInParallel() => View();

        [HttpPost]
        public async Task<ViewResult> SumInParallel(ParallelElement elements)
        {
            ParallelTime ResultTime = await ParallelSerivice.SumElementsList(elements.NumberOfElements).ConfigureAwait(false);
            setHeaders();
            ViewData["ParallelResult"] = ResultTime;
            return View();
        }

         private void setHeaders()
        {
            ViewBag.ParellSumRegular = "Regular Sum";
            ViewBag.ParellSumAsParallel = "Parralel";
            ViewBag.ParellSumAsAggregate = "Aggregate"; 
        }
    }
}