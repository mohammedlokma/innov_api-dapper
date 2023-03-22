using innov_web.Models.DTO;
using innov_web.Services;
using innov_web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace innov_web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        public async Task<IActionResult> Index()
        {
            List<GroupDto> list = new();

            var response = await _groupService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<GroupDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> GroupCreate()
        {
            GroupDto groupDto = new();
            return View(groupDto);
        }
        public async Task<IActionResult> GroupUpdate(int groupId)
        {
            var response = await _groupService.GetGroupAsync<APIResponse>(groupId);
            
            var groupDto = JsonConvert.DeserializeObject<GroupDto>(Convert.ToString(response.Result));
            return View(groupDto);
        }
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var response = await _groupService.DeleteGroupAsync<APIResponse>(groupId);
            
             return RedirectToAction(nameof(Index));
            
            
        }

        public async Task<IActionResult> UnDeleteGroup(int groupId)
        {
            var response = await _groupService.UnDeleteGroupAsync<APIResponse>(groupId);

            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupUpdate(GroupDto model)
        {
            // var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                
                var response = await _groupService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Group updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupCreate(GroupDto model)
        {
            // var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
              
                model.Connection.Server = (model.Connection.Server).Replace(@"\\", @"\");
                var response = await _groupService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Group created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        public async Task<IActionResult> GroupVerbs(int groupId)
        {

            var groupVerbs = new GroupVerbsDto();
            var response = await _groupService.GetGroupVerbsAsync<APIResponse>(groupId);
            if (response != null && response.IsSuccess)
            {
                 groupVerbs = JsonConvert.DeserializeObject<GroupVerbsDto>(Convert.ToString(response.Result));
            }
            groupVerbs.Id = groupId;
            return View(groupVerbs);
        }

    }
}
