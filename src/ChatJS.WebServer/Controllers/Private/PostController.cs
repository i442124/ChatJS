using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Posts.Commands;
using ChatJS.Models.Chatlogs;
using ChatJS.Models.Messages;
using ChatJS.Models.Posts;
using ChatJS.WebServer.Hubs;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatJS.WebServer.Controllers.Private
{
    [Authorize]
    [ApiController]
    [Route("api/private/posts")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IContextService _contextService;
        private readonly INotificationService _notificationService;
        private readonly IChatlogModelBuilder _chatlogModelBuilder;

        public PostController(
            IPostService postService,
            IContextService contextService,
            INotificationService notificationService,
            IChatlogModelBuilder chatlogModelBuilder)
        {
            _postService = postService;
            _contextService = contextService;
            _notificationService = notificationService;
            _chatlogModelBuilder = chatlogModelBuilder;
        }

        [HttpPost("create")]
        public async Task<IActionResult> OnPostAsync(FormComponentModel model)
        {
            var command = new CreatePost
            {
                MessageId = model.Message.Id,
                ChatroomId = model.Chatroom.Id
            };

            await _postService.CreateAsync(command);
            await _notificationService.PublishAsync("GetNewPost", command.ChatroomId, await _chatlogModelBuilder.BuildMessageModelAsync(command.ChatroomId, command.MessageId));

            return Ok();
        }

        [HttpDelete("delete/{postId}")]
        public async Task<IActionResult> OnDeleteAsync(Guid postId)
        {
            var command = new DeletePost
            {
                Id = postId
            };

            await _postService.DeleteAsync(command);
            return Ok();
        }
    }
}
