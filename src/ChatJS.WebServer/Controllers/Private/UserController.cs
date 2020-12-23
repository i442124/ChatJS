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
        private readonly IContextService _context;
        private readonly IUserModelBuilder _builder;

        public UserController(
            IContextService context,
            IUserModelBuilder builder)
        {
            _context = context;
            _builder = builder;
        }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _context.CurrentUserAsync();
            return Json(await _builder.BuildAreaAsync(user.Id));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            return Json(await _builder.BuildAreaAsync(userId));
        }
    }
}
