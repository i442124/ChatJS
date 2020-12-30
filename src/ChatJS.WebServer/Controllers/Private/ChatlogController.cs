using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Models;
using ChatJS.Models.Chatlogs;
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
        private readonly IContextService _contextService;
        private readonly IChatlogModelBuilder _chatlogModelBuilder;

        public ChatlogController(
            IContextService contextService,
            IChatlogModelBuilder chatlogModelBuilder)
        {
            _contextService = contextService;
            _chatlogModelBuilder = chatlogModelBuilder;
        }

        [HttpGet("{chatroomId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatlogModelBuilder.BuildChatlogPageModelAsync(user.Id, chatroomId));
        }

        [HttpGet("anonymous/{chatroomId}")]
        public async Task<IActionResult> OnGetAnonymousAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatlogModelBuilder.BuildChatlogPageModelAnonymousAsync(user.Id, chatroomId));
        }

        [HttpGet("{chatroomId}/{messageId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatroomId, Guid messageId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatlogModelBuilder.BuildMessageModelAsync(user.Id, chatroomId, messageId));
        }

        [HttpGet("anonymous/{chatroomId}/{messageId}")]
        public async Task<IActionResult> OnGetAnonymousAsync(Guid chatroomId, Guid messageId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatlogModelBuilder.BuildMessageModelAnonymousAsync(user.Id, chatroomId, messageId));
        }
    }
}
