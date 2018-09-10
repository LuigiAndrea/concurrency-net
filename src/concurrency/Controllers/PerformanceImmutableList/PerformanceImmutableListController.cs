using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Concurrency.Models.PerfornabceImmutableList;
using static Concurrency.Services.PerformanceImmutableList.PerformanceImmutableListSerivice;

namespace Concurrency.Controllers
{
    public class PerformanceImmutableListController : Controller
    {
        [HttpGet]
        public IActionResult RunPerformanceTest() => View();

        [HttpPost]
        public async Task<ViewResult> RunPerformanceTest(ImmutableListTest test)
        {
            PerformanceResult ResultTime = await RunTest(test.NumberOfElements);
            ViewData["PerformanceResult"] = ResultTime;
         
            return View();
        }
    }
}
