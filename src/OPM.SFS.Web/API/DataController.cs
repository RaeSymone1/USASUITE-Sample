using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//API template to be used for future APIs to share data with USAJOBS/ATP
//For security we should implement an authorization endpoint utilizing
//OAuth + OIDC, with the client credential flow for system to system
//IdentityServer is no longer open source. An alternative is OpenIddict


namespace OPM.SFS.Web.Controllers { 

    [Route("api/data")]
    [Produces("application/json")]
    [ApiController]
    public class DataController : ControllerBase
    {
        // GET: api/data
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/data/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/data
        [HttpPost]
        public void Post([FromBody] Task value)
        {
            string test = value.TaskName;
        }

        // PUT api/data/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/data/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


    public class Task
    {
        public string TaskName { get; set; }
    }
}
