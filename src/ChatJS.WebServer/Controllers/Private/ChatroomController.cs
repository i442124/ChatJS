using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Models;
using ChatJS.Models.Chatrooms;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Private
{
    [Authorize]
    [ApiController]
    [Route("api/private/chatrooms")]
    public class ChatroomController : Controller
    {
        private readonly IContextService _contextService;
        private readonly IChatroomModelBuilder _chatroomBuilder;

        public ChatroomController(
            IContextService contextService,
            IChatroomModelBuilder chatroomBuilder)
        {
            _contextService = contextService;
            _chatroomBuilder = chatroomBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatroomBuilder.BuildAllAsync(user.Id));
        }

        [HttpGet("{chatlogId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatroomBuilder.BuildAsync(user.Id, chatroomId));
        }
    }
}
