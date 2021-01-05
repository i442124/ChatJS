using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Posts;
using ChatJS.Domain.Posts.Commands;
using ChatJS.Models.Posts;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Protected
{
    [Authorize]
    [ApiController]
    [Route("api/protected/posts")]
    public class PostController : Controller
    {
        private readonly IPostRules _postRules;
        private readonly IPostService _postService;
        private readonly IPostModelBuilder _postModelBuilder;

        private readonly IContextService _contextService;
        private readonly INotificationService _notifyService;

        public PostController(
            IPostRules postRules,
            IPostService postService,
            IPostModelBuilder postModelBuilder,
            IContextService contextService,
            INotificationService notifyService)
        {
            _postRules = postRules;
            _postService = postService;
            _postModelBuilder = postModelBuilder;

            _contextService = contextService;
            _notifyService = notifyService;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetAsync(Guid postId)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _postRules.IsAuthorizedForPostAsync(user.Id, postId))
            {
                var messageModel = await _postModelBuilder.BuildMessageModelAsync(postId);
                return Ok(messageModel);
            }

            return Unauthorized();
        }

        [HttpGet("all/{chatroomId}")]
        public async Task<IActionResult> GetAllAsync(Guid chatroomId)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _postRules.IsAuthorizedForChatroomAsync(user.Id, chatroomId))
            {
                var postPageModel = await _postModelBuilder.BuildPostPageModelAsync(chatroomId);
                return Ok(postPageModel);
            }

            return Unauthorized();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _postRules.IsAuthorizedForChatroomAsync(user.Id, model.Chatroom.Id) &&
                await _postRules.IsAuthorizedToPostAsync(user.Id, model.Chatroom.Id))
            {
                var command = new CreatePost
                {
                    MessageId = model.Message.Id,
                    ChatroomId = model.Chatroom.Id,
                };

                await _postService.CreateAsync(command);
                await _notifyService.PublishScopedAsync(
                    "CreatePost",
                    model.Chatroom.Id,
                    await _postModelBuilder.BuildMessageModelAsync(command.Id));

                return Ok();
            }

            return Unauthorized();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _postRules.IsAuthorizedForChatroomAsync(user.Id, model.Chatroom.Id) &&
                await _postRules.IsAuthorizedToPostAsync(user.Id, model.Message.Id))
            {
                var command = new DeletePost
                {
                    Id = model.Id
                };

                await _postService.DeleteAsync(command);
                await _notifyService.PublishScopedAsync(
                    "DeletePost",
                    model.Chatroom.Id,
                    await _postModelBuilder.BuildMessageModelAsync(command.Id));

                return Ok();
            }

            return Unauthorized();
        }
    }
}
