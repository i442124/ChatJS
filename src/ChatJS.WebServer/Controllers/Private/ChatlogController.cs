using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Models;
using ChatJS.Models.Chatlog;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Private
{
    [Authorize]
    [ApiController]
    [Route("api/private/chatlogs")]
    public class ChatlogController : Controller
    {
        private readonly IContextService _context;
        private readonly IChatlogModelBuilder _builder;

        public ChatlogController(
            IContextService context,
            IChatlogModelBuilder builder)
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

        [HttpGet("{chatlogId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatlogId)
        {
            var user = await _context.CurrentUserAsync();
            return Json(await _builder.BuildEntryAsync(user.Id, chatlogId));
        }
    }
}
