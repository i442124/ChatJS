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
        private readonly IContextService _contextService;
        private readonly IMessageModelBuilder _messageModelBuilder;

        public MessageController(
            IContextService contextService,
            IMessageModelBuilder messageModelBuilder)
        {
            _contextService = contextService;
            _messageModelBuilder = messageModelBuilder;
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> OnGetAsync(Guid messageId)
        {
            return Json(await _messageModelBuilder.BuildMessagePageModelAsync(messageId));
        }
    }
}
