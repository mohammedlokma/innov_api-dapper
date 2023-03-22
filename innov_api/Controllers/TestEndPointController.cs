using innov_api.Models.DTOs;
using innov_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using task_api.Models;
using AutoMapper;
using innov_api.Data;
using System.Data;
using System.Dynamic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Security.Cryptography;

namespace innov_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEndPointController : ControllerBase
    {
        protected ApiRespose _response;
        private ApplicationDbContext _dbContext;
        private IMapper _mapper;
        public TestEndPointController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _response = new();
            _mapper = mapper;
        }
        [HttpGet("Select")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> Select([FromQuery] DbConfigDto dbConfig)
        {
            try
            {


                if (dbConfig == null)
                {
                    return BadRequest();
                }
                var convertedObj = new Dictionary<string, dynamic>();
                if (dbConfig.paramters != "[]")
                {
                     convertedObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(dbConfig.paramters);
                }
                var connectionString = dbConfig.ConnectionString;
                var query = dbConfig.QueryStatement;

                IDbConnection db = new SqlConnection(connectionString);
             
                var parameters = new DynamicParameters();
                if (convertedObj.Keys.Count > 0)
                {
                    foreach (var param in convertedObj.Keys)
                    {
                        var value = convertedObj[param];
                        parameters.Add('@' + param, convertedObj[param]);

                    }
                }
                var queryResults = (from row in await db.QueryAsync(query,parameters)
                              select (IDictionary<string, object>)row).AsList();
            
                _response.Result = queryResults;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("Execute")]
        [HttpPost("Execute")]
        [HttpDelete("Execute")]
        [HttpPut("Execute")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> Execute([FromQuery] DbConfigDto dbConfig)
        {
            try
            {

                if (dbConfig == null)
                {
                    return BadRequest();
                }
                var convertedObj = new Dictionary<string, dynamic>();
                if (dbConfig.paramters != "[]")
                {
                    convertedObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(dbConfig.paramters);
                }
                var connectionString = dbConfig.ConnectionString;

                var query = dbConfig.QueryStatement;

                IDbConnection db = new SqlConnection(connectionString);

                var parameters = new DynamicParameters();
                if (convertedObj.Keys.Count > 0)
                {
                    foreach (var param in convertedObj.Keys)
                    {
                        var value = convertedObj[param];
                        parameters.Add('@' + param, convertedObj[param]);

                    }
                }
                db.Execute(query, parameters);

                _response.Result =  dbConfig.MethodType + " Successfuly";
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response.Result);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
       
    }
}
