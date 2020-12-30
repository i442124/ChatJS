using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
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
        private readonly IMessageService _messageService;
        private readonly IMessageModelBuilder _messageModelBuilder;

        public MessageController(
            IContextService contextService,
            IMessageService messageService,
            IMessageModelBuilder messageModelBuilder)
        {
            _contextService = contextService;
            _messageService = messageService;
            _messageModelBuilder = messageModelBuilder;
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> OnGetAsync(Guid messageId)
        {
            return Json(await _messageModelBuilder.BuildMessagePageModelAsync(messageId));
        }

        [HttpPost("create")]
        public async Task<IActionResult> OnPostAsync(ForumComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();

            var command = new CreateMessage
            {
                UserId = user.Id,
                Content = model.Content,
                Attachment = model.Attachment
            };

            await _messageService.CreateAsync(command);
            return Ok(await _messageModelBuilder.BuildMessagePageModelAsync(command.Id));
        }

        [HttpPut("edit/{messageId}")]
        public async Task<IActionResult> OnPutAsync(Guid messageId, ForumComponentModel model)
        {
            var command = new UpdateMessage
            {
                Id = messageId,
                Content = model.Content,
                Attachment = model.Attachment
            };

            await _messageService.UpdateAsync(command);
            return Ok(await _messageModelBuilder.BuildMessagePageModelAsync(command.Id));
        }

        [HttpDelete("delete/{messageId}")]
        public async Task<IActionResult> OnDeleteAsync(Guid messageId)
        {
            var command = new DeleteMessage
            {
                Id = messageId
            };

            var messagePageModel = await _messageModelBuilder
            .BuildMessagePageModelAsync(messageId);

            await _messageService.DeleteAsync(command);
            return Ok(messagePageModel);
        }
    }
}
