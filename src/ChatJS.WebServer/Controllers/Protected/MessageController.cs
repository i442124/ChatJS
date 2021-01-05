using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
using ChatJS.Models.Messages;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Protected
{
    [Authorize]
    [ApiController]
    [Route("api/protected/messages")]
    public class MessageController : Controller
    {
        private readonly IMessageRules _messageRules;
        private readonly IMessageService _messageService;
        private readonly IMessageModelBuilder _messageModelBuilder;

        private readonly IContextService _contextService;
        private readonly INotificationService _notifyService;

        public MessageController(
            IMessageRules messageRules,
            IMessageService messageService,
            IMessageModelBuilder messageModelBuilder,
            IContextService contextService,
            INotificationService notifyService)
        {
            _messageRules = messageRules;
            _messageService = messageService;
            _messageModelBuilder = messageModelBuilder;

            _notifyService = notifyService;
            _contextService = contextService;
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetAsync(Guid messageId)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _messageRules.IsAuthorizedAsync(user.Id, messageId))
            {
                return Ok(await _messageModelBuilder.BuildMessagePageModelAsync(messageId));
            }

            return Unauthorized();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(ForumComponentModel model)
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

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(Guid messageId, ForumComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _messageRules.IsAuthorizedAsync(user.Id, messageId))
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

            return Unauthorized();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(Guid messageId)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _messageRules.IsAuthorizedAsync(user.Id, messageId))
            {
                var command = new DeleteMessage
                {
                    Id = messageId
                };

                await _messageService.DeleteAsync(command);
                return Ok();
            }

            return Unauthorized();
        }
    }
}
