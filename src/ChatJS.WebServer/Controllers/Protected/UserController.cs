using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Users;
using ChatJS.Models.Users;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Protected
{
    [Authorize]
    [ApiController]
    [Route("api/protected/users")]
    public class UserController : Controller
    {
        private readonly IUserRules _userRules;
        private readonly IUserService _userService;
        private readonly IUserModelBuilder _userModelBuilder;

        private readonly IContextService _contextService;
        private readonly INotificationService _notifyService;

        public UserController(
            IUserRules userRules,
            IUserService userService,
            IUserModelBuilder userModelBuilder,
            IContextService contextService,
            INotificationService notifyService)
        {
            _userRules = userRules;
            _userService = userService;
            _userModelBuilder = userModelBuilder;

            _contextService = contextService;
            _notifyService = notifyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _userRules.IsValidAsync(user.Id))
            {
                var userModel = await _userModelBuilder.BuildUserModelAsync(user.Id);
                return Json(userModel);
            }

            return Unauthorized();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _userRules.IsValidAsync(user.Id))
            {
                var userPageModel = await _userModelBuilder.BuildUserPageModelAsync(user.Id);
                return Json(userPageModel);
            }

            return Unauthorized();
        }
    }
}
