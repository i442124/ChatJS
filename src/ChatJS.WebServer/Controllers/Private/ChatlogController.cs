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
            IChatlogModelBuilder chatroomBuilder)
        {
            _contextService = contextService;
            _chatlogModelBuilder = chatroomBuilder;
        }

        [HttpGet("{chatroomId}")]
        public async Task<IActionResult> OnGetAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            return Json(await _chatlogModelBuilder.BuildAsync(user.Id, chatroomId));
        }
    }
}
