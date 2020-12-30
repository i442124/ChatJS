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
        private readonly IChatroomModelBuilder _chatroomModelBuilder;

        public ChatroomController(
            IContextService contextService,
            IChatroomModelBuilder chatroomModelBuilder)
        {
            _contextService = contextService;
            _chatroomModelBuilder = chatroomModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatroomModelBuilder.BuildChatroomPageModelAsync(user.Id));
        }

        [HttpGet("{chatlogId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatroomModelBuilder.BuildChatroomModelAsync(user.Id, chatroomId));
        }
    }
}
