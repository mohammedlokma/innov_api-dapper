using AutoMapper;
using innov_api.Data;
using innov_api.Models;
using innov_api.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Dynamic;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using task_api.Models;
using static Dapper.SqlMapper;


namespace innov_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        protected ApiRespose _response;
        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public GroupController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet("GetGroups")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> GetGroups()
        {
            try
            {

                IEnumerable<Group> groups = await _dbContext.Groups.ToListAsync();
                var groupsDto = _mapper.Map<List<GroupDto>>(groups);
                _response.Result = groupsDto;
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpGet("GetGroup")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> GetGroup(int groupId)
        {
            try
            {

                var group = await _dbContext.Groups.AsNoTracking().Include(i=>i.Connection).FirstOrDefaultAsync(i=>i.Id == groupId);
                var groupDto = _mapper.Map<GroupDto>(group);
                _response.Result = groupDto;
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpGet("GetDeletedGroups")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> GetDeletedGroups()
        {
            try
            {

                IEnumerable<Group> groups = await _dbContext.Groups.ToListAsync();
                var groupsDto = _mapper.Map<List<GroupDto>>(groups);
                _response.Result = groupsDto;
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpPost("CreateGroup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateGroup([FromBody] GroupDto groupCreateDto)
        {
            try
            {
               
                if (await _dbContext.Groups.AsNoTracking().SingleOrDefaultAsync(i=>i.Name == groupCreateDto.Name) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Name already Exists!");
                    return BadRequest(ModelState);
                }

                if (groupCreateDto == null)
                {
                    return BadRequest();
                }
                var transaction = _dbContext.Database.BeginTransaction();
                try

                {
                    var connectionString = "Data Source = " + groupCreateDto.Connection.Server + "; Database = " +
                        groupCreateDto.Connection.DbName + "; User ID = " + groupCreateDto.Connection.UserName +
                        "; Password =" + groupCreateDto.Connection.Password +
                        "; Integrated Security =" + groupCreateDto.Connection.IntegratedSecurity
                        + "; Trusted_Connection =" + groupCreateDto.Connection.TrustedConnection + ";";

                    Connection connection = _mapper.Map<Connection>(groupCreateDto.Connection);

                    Group group = _mapper.Map<Group>(groupCreateDto);
                    group.ConnectionString = connectionString;
                    await _dbContext.Groups.AddAsync(group);
                    await _dbContext.SaveChangesAsync();
                    connection.GroupId = group.Id;
                    await _dbContext.Connections.AddAsync(connection);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                }
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
        [HttpPut("UpdateGroup")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> UpdateGroup(GroupDto updateDto)
        {
            try
            {

                var group = await _dbContext.Groups.FirstOrDefaultAsync(i => i.Id == updateDto.Id);
                if (group is null)
                {
                    return NotFound("Group Not Found");
                }
                var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    var connection = _mapper.Map<Connection>(updateDto.Connection);
                    _dbContext.Connections.Update(connection);
                    var connectionString = "Data Source = " + updateDto.Connection.Server + "; Database = " +
                       updateDto.Connection.DbName + "; User ID = " + updateDto.Connection.UserName +
                       "; Password =" + updateDto.Connection.Password +
                       "; Integrated Security =" + updateDto.Connection.IntegratedSecurity
                       + "; Trusted_Connection =" + updateDto.Connection.TrustedConnection + ";";
                    group.ConnectionString = connectionString;
                    group.Name = updateDto.Name;
                    group.DbType = updateDto.DbType;

                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages
                         = new List<string>() { ex.ToString() };

                    transaction.Rollback();
                }
                
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpDelete("DeleteGroup")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> DeleteGroup(int groupId)
        {
            try
            {

                var group = await _dbContext.Groups.FirstOrDefaultAsync(i => i.Id == groupId);
                if (group is null)
                {
                    return NotFound("Group Not Found");
                }
                group.Deleted = true;
                await _dbContext.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpDelete("UnDeleteGroup")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> UnDeleteGroup(int groupId)
        {
            try
            {

                var group = await _dbContext.Groups.FirstOrDefaultAsync(i => i.Id == groupId);
                if (group is null)
                {
                    return NotFound("Group Not Found");
                }
                group.Deleted = false;
                await _dbContext.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpGet("GetGroupVerbs")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiRespose>> GetGroupVerbs(int groupId)
        {
            try
            {
                var group = await _dbContext.Groups.FirstOrDefaultAsync(i => i.Id == groupId); 
                IEnumerable<Verb> verbs = await _dbContext.Verbs.Where(i => i.GroupId == groupId).ToListAsync();
                
               var verbsDto = _mapper.Map<List<VerbDto>>(verbs);
                var groupVerbsDto = new GroupVerbsDto()
                {
                    GroupName = group.Name,
                    VerbsDto = verbsDto

                };
                _response.Result = groupVerbsDto;
                _response.StatusCode = HttpStatusCode.OK;
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
       

    }
}
