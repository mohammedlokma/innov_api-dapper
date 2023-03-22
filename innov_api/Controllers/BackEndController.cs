using innov_api.Data;
using innov_api.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace innov_api.Controllers
{
    [Route("[controller]/{group}/{name}")]
    [ApiController]
    public class BackEndController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public BackEndController(ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }
       
        [HttpGet]
        [HttpPost]
        [HttpDelete]
        [HttpPut]

        //[Consumes("application/json")]
        public async Task<IActionResult> Index()
        {
            var schema = HttpContext.Request.Scheme.ToString();
            var host = HttpContext.Request.Host.ToString();
            var link = HttpContext.Request.Path.ToString();
            var fulLink = schema + "://" + host + link;
            var queryString = HttpContext.Request.QueryString;
            var endPoint = await _dbContext.Verbs.Include("Group").FirstOrDefaultAsync(i => i.Link == fulLink);

            if (endPoint is not null)
            {
                var paramters = new List<string>();
                var data = JsonConvert.SerializeObject(paramters);

              

                var dbObject = new DbConfigDto
                {
                    ConnectionString = endPoint.Group.ConnectionString,
                    MethodType = endPoint.Type,
                    QueryStatement = endPoint.QueryStatement,
                    paramters = data

                };
                
                if (endPoint.Type == "GET")
                {
                    return RedirectToAction("Select", "TestEndPoint", new RouteValueDictionary(dbObject));

                }
                else
                {
                    return RedirectToAction("Execute", "TestEndPoint", new RouteValueDictionary(dbObject));

                }
            }
            return BadRequest("EndPoint is not correct");
        }
        [HttpGet]
        [HttpPost]
        [HttpDelete]
        [HttpPut]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/json")]
        public async Task<IActionResult> IndexByFilter([FromBody] Dictionary<string, string>? paramters = null)
        {
            var schema = HttpContext.Request.Scheme.ToString();
            var host = HttpContext.Request.Host.ToString();
            var link = HttpContext.Request.Path.ToString();
            var fulLink = schema + "://" + host + link;
            var queryString = HttpContext.Request.QueryString;
            var endPoint = await _dbContext.Verbs.Include("Group").FirstOrDefaultAsync(i => i.Link == fulLink);

            if (endPoint is not null)
            {
                string data = null;
                if (paramters is not null)
                {
                    data = JsonConvert.SerializeObject(paramters);

                }

                var dbObject = new DbConfigDto
                {
                    ConnectionString = endPoint.Group.ConnectionString,
                    MethodType = endPoint.Type,
                    QueryStatement = endPoint.QueryStatement,
                    paramters = data

                };

                if (endPoint.Type == "GET")
                {
                    return RedirectToAction("Select", "TestEndPoint", new RouteValueDictionary(dbObject));

                }
                else
                {
                    return RedirectToAction("Execute", "TestEndPoint", new RouteValueDictionary(dbObject));

                }
            }
            return BadRequest("EndPoint is not correct");
        }
    }
}
