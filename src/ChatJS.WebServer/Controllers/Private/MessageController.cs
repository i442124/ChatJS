using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Models;
using ChatJS.Models.Messages;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Private
{
    [Authorize]
    [ApiController]
    [Route("api/private/messages")]
    public class MessageController : Controller
    {
        private readonly IContextService _context;
        private readonly IMessageModelBuilder _builder;

        public MessageController(
            IContextService context,
            IMessageModelBuilder builder)
        {
            _context = context;
            _builder = builder;
        }

        [HttpGet("{chatlogId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatlogId)
        {
            var user = await _context.CurrentUserAsync();
            return Json(await _builder.BuildAreaAsync(user.Id, chatlogId));
        }

        [HttpGet("{chatlogId}/{index}")]
        public async Task<IActionResult> OnGetAsync(Guid chatlogId, int index)
        {
            var user = await _context.CurrentUserAsync();
            return Json(await _builder.BuildEntryAsync(user.Id, chatlogId, index));
        }
    }
}
