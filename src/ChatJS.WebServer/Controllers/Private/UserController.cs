using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ChatJS.Models.Users;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Private
{
    [Authorize]
    [ApiController]
    [Route("api/private/users")]
    public class UserController : Controller
    {
        private readonly IContextService _contextService;
        private readonly IUserModelBuilder _userModelBuilder;

        public UserController(
            IContextService contextService,
            IUserModelBuilder userModelBuilder)
        {
            _contextService = contextService;
            _userModelBuilder = userModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _userModelBuilder.BuildUserPageModelAsync(user.Id));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            return Json(await _userModelBuilder.BuildUserModelAsync(userId));
        }
    }
}
