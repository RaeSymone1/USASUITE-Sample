using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPM.SFS.Web.SharedCode;
using System.Threading.Tasks;

namespace OPM.SFS.Web.API
{
    [ApiKey]
    [Route("api/tasks")]
    [Produces("application/json")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        private readonly ITaskHandler _taskHandler;
        

        public TasksController(ITaskHandler handler)
        {
            _taskHandler = handler;
            
        }

        [HttpPost]
        public async Task<JsonResult> PostTaskAsync([FromBody] TaskItem item)
        {
            var success = await _taskHandler.RunTaskAsync(item.TaskName, item.AccountType, item.Batch);
            if(success)
                return new JsonResult("success");
            else
                return new JsonResult("error");
        }

      
        

      
    }

    public class TaskItem
    {
        public string TaskName { get; set; }
        public string AccountType { get; set; }
        public string Batch { get; set; }
    }
}
