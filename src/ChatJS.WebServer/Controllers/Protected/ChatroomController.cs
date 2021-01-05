using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Models.Chatrooms;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Protected
{
    [Authorize]
    [ApiController]
    [Route("api/protected/chatrooms")]
    public class ChatroomController : Controller
    {
        private readonly IChatroomRules _chatroomRules;
        private readonly IChatroomService _chatroomService;
        private readonly IChatroomModelBuilder _chatroomModelBuilder;

        private readonly IContextService _contextService;
        private readonly IMembershipService _membershipService;
        private readonly INotificationService _notifyService;

        public ChatroomController(
            IChatroomRules chatroomRules,
            IChatroomService chatroomService,
            IChatroomModelBuilder chatroomModelBuilder,
            IContextService contextService,
            INotificationService notifyService,
            IMembershipService membershipService)
        {
            _chatroomRules = chatroomRules;
            _chatroomService = chatroomService;
            _chatroomModelBuilder = chatroomModelBuilder;

            _notifyService = notifyService;
            _contextService = contextService;
            _membershipService = membershipService;
        }

        [HttpGet("{chatroomId}")]
        public async Task<IActionResult> GetAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _chatroomRules.IsAuthorizedAsync(user.Id, chatroomId))
            {
                return Ok(await _chatroomModelBuilder.BuildChatroomPageModelAsync(chatroomId));
            }

            return Unauthorized();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            var command = new CreateChatroom
            {
                Name = model.Name,
                NameCaption = model.NameCaption
            };

            await _chatroomService.CreateAsync(command);
            await _membershipService.CreateAsync(new CreateMembership
            {
                UserId = user.Id,
                ChatroomId = command.Id
            });

            var chatroom = new { command.Id };
            var chatroomModel = await _chatroomModelBuilder.BuildChatroomModelAsync(chatroom.Id);

            await _notifyService.SubscribeAsync(user.Id, command.Id);
            await _notifyService.PublishAsync("CreateChatroom", command.Id, chatroomModel);

            return Ok(chatroomModel);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _chatroomRules.IsAuthorizedAsync(user.Id, model.Id))
            {
                var command = new UpdateChatroom
                {
                    Id = model.Id,
                    Name = model.Name,
                    NameCaption = model.NameCaption
                };

                var chatroom = new { command.Id };
                var chatroomModel = await _chatroomModelBuilder.BuildChatroomModelAsync(chatroom.Id);

                await _chatroomService.UpdateAsync(command);
                await _notifyService.PublishAsync("UpdateChatroom", model.Id, chatroomModel);

                return Ok(chatroomModel);
            }

            return Unauthorized();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _chatroomRules.IsAuthorizedAsync(user.Id, model.Id))
            {
                var command = new DeleteChatroom
                {
                    Id = model.Id
                };

                var chatroom = new { command.Id };
                var chatroomModel = await _chatroomModelBuilder.BuildChatroomModelAsync(chatroom.Id);

                await _chatroomService.DeleteAsync(command);
                await _notifyService.PublishAsync("DeleteChatroom", command.Id, chatroom);

                return Ok(chatroomModel);
            }

            return Unauthorized();
        }
    }
}
