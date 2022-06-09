using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZelosFramework.NLP_Core;

namespace ZelosFrontend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecutedScriptsController : Controller
    {
        private readonly ILogger<ScriptController> logger;

        public ExecutedScriptsController(ILogger<ScriptController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<ScriptRun> Get()
        {
            var result = new List<ScriptRun>();

            var scriptExample = new Script();
            scriptExample.Name = "Test123";

            for (int i = 0; i < 20; i++)
            {
                var scriptRun = new ScriptRun();

                scriptRun.Script = scriptExample;

                Random gen = new Random();
                DateTime start = new DateTime(2020, 1, 1);
                int range = (DateTime.Today - start).Days;
                
                scriptRun.ExecutionTime =  start.AddDays(gen.Next(range));

                range = Enum.GetValues(typeof(SuccessState)).Length;

                scriptRun.SuccessState = (SuccessState) gen.Next(range);

                result.Add(scriptRun);
            }

            return result;
        }
    }
}
