using Microsoft.AspNetCore.Mvc;
using to_do_list.Domain.Entities;
using to_do_list.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application;

namespace to_do_list.Controllers
{
    [Route("api/dolist")]
    [ApiController]
    [Authorize]
    public class controllers_api_dolist : ControllerBase
    {
        private readonly DolistService _dolistService;

        public controllers_api_dolist(DolistService dolistService)
        {
            _dolistService = dolistService;
        }

        [Flags]
        public enum enum_permission
        {
            all_permission = -1,
            shoow_all_dolist = 1,
            show_dolist_bycategory = 2,
            show_dolist_priority = 4,
            show_dolist_list_id = 8,
            change_state_dolist = 16,
            add_users = 32,
            add_dolist = 64,
            update_dolist = 128,
            remove_dolist = 256
        }

        private int GetUserPermission()
        {
            var permClaim = User.Claims.FirstOrDefault(c => c.Type == "permission");
            return permClaim != null && int.TryParse(permClaim.Value, out int perm) ? perm : 0;
        }

        private int GetUserId()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return idClaim != null && int.TryParse(idClaim.Value, out int userId) ? userId : -1;
        }

        [HttpGet("ShowDolistAsync")]
        public async Task<ActionResult<IEnumerable<Dolist>>> ShowDolistAsync(int UserID,bool? sort_by, int page_number, int page_size)
        {
            int userId = GetUserId();
            if (userId == -1)
                return BadRequest("you do not sign in");

            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.shoow_all_dolist))
                return BadRequest("You don't have permission");

            var dolists = await _dolistService.Show_All_DolistAsync(UserID, sort_by, page_number, page_size);
            return Ok(dolists);
        }

        [HttpGet("ShowDolistByCategoryAsync")]
        public async Task<ActionResult<IEnumerable<Dolist>>> ShowDolistByCategoryAsync(int UserID,string category)
        {
            int userId = GetUserId();
            if (userId == -1)
                return BadRequest("Invalid user");

            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.show_dolist_bycategory))
                return BadRequest("You don't have permission");

            var dolists = await _dolistService.Show_Dolist_Filter_By_CategoryAsync(UserID, category);
            return Ok(dolists);
        }

        [HttpGet("ShowDolistByPriorityAsync")]
        public async Task<ActionResult<IEnumerable<Dolist>>> ShowDolistByPriorityAsync(int UserID, int priority)
        {
            int userId = GetUserId();
            if (userId == -1)
                return BadRequest("Invalid user");

            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.show_dolist_priority))
                return BadRequest("You don't have permission");

            var dolists = await _dolistService.Show_Dolist_Filter_By_PriorityAsync(UserID, priority);
            return Ok(dolists);
        }

        [HttpGet("ShowDolistByIdAsync")]
        public async Task<ActionResult<Dolist>> ShowDolistByIdAsync(int list_id)
        {
            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.show_dolist_list_id))
                return BadRequest("You don't have permission");

            var dolist = await _dolistService.Show_Dolist_Filter_By_IDAsync(list_id);
            if (dolist == null)
                return NotFound("List not found");

            return Ok(dolist);
        }

        [HttpPut("ChangeStateDolistAsync")]
        public async Task<ActionResult> ChangeStateDolistAsync(int list_id, bool completed)
        {
            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.change_state_dolist))
                return BadRequest("You don't have permission");

            bool result = await _dolistService.Change_State_DolistAsync(list_id, completed);
            return result ? Ok("Updated successfully") : BadRequest("List not found");
        }

        [HttpPost("AddDolistAsync")]
        public async Task<ActionResult> AddDolistAsync(string list_title, string list_body, bool completed, string category, int priority)
        {
            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.add_dolist))
                return BadRequest("You don't have permission");

            int userId = GetUserId();
            if (userId == -1)
                return BadRequest("Invalid user");

            int result = await _dolistService.Add_DolistAsync(userId, list_title, list_body, completed, category, priority);
            return result != -1 ? Ok($"Added successfully with ID {result}") : BadRequest("Error adding the list");
        }

        [HttpPut("UpdateDolistAsync")]
        public async Task<ActionResult> UpdateDolistAsync(int list_id, string list_title, string list_body, bool completed, string category, int priority)
        {
            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.update_dolist))
                return BadRequest("You don't have permission");

            int userId = GetUserId();
            if (userId == -1)
                return BadRequest("Invalid user");

            bool result = await _dolistService.Update_DolistAsync(list_id, userId, list_title, list_body, completed, category, priority);
            return result ? Ok("Updated successfully") : BadRequest("List not found");
        }

        [HttpDelete("RemoveDolistAsync")]
        public async Task<ActionResult> RemoveDolistAsync(int list_id)
        {
            if (!((enum_permission)GetUserPermission()).HasFlag(enum_permission.remove_dolist))
                return BadRequest("You don't have permission");

            bool result = await _dolistService.Delete_DolistAsync(list_id);
            return result ? Ok("Deleted successfully") : BadRequest("List not found");
        }
    }
}
