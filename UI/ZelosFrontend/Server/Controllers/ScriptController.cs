using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZelosFramework.NLP_Core;
using Persisting;

namespace ZelosFrontend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScriptController : ControllerBase
    {
        private readonly ILogger<ScriptController> logger;

        private readonly IScriptRepository scriptRepository;

        public ScriptController(ILogger<ScriptController> logger, IScriptRepository scriptRepository)
        {
            this.logger = logger;
            this.scriptRepository = scriptRepository;
        }

        [HttpGet]
        public IEnumerable<Script> Get()
        {
            return this.scriptRepository.GetScripts();
        }

        [HttpPut]
        public ActionResult<Script> Put([FromBody] Script script)
        {
            if (script == null)
            {
                return BadRequest();
            }

            var docFound = scriptRepository.GetScriptByName(script.Name);

            if (docFound != null)
            {
                ModelState.AddModelError("Name", "Script with this name already in use");
                return BadRequest(ModelState);
            }

            

            var createdDoc = scriptRepository.AddScript(script);

            return Created(nameof(Put), createdDoc);
        }
    }
}
