using innov_web.Const;
using innov_web.Models.DTO;
using innov_web.Services.Interfaces;

namespace innov_web.Services
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string groupUrl;
        public GroupService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            groupUrl = configuration.GetValue<string>("ServiceUrls:InnovAPI");
        }
        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = groupUrl + "/api/Group/GetGroups",
            });

        }
        public Task<T> GetGroupAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = groupUrl + "/api/Group/GetGroup?groupId=" + id,
            });

        }
        public Task<T> CreateAsync<T>(GroupDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = groupUrl + "/api/Group/CreateGroup"

            });
        }
        public Task<T> UpdateAsync<T>(GroupDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = groupUrl + "/api/Group/UpdateGroup"

            });
        }
        public Task<T> DeleteGroupAsync<T>(int groupId)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = groupUrl + "/api/Group/DeleteGroup?groupId=" + groupId

            });
        }
        public Task<T> UnDeleteGroupAsync<T>(int groupId)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = groupUrl + "/api/Group/UnDeleteGroup?groupId=" + groupId

            });
        }
        public Task<T> GetGroupVerbsAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = groupUrl + "/api/Group/GetGroupVerbs?groupId=" + id,
            });

        }
    }
}
