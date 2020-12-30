using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Posts.Commands;
using ChatJS.Models.Posts;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Private
{
    [Authorize]
    [ApiController]
    [Route("api/private/posts")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IContextService _contextService;
        private readonly IPostModelBuilder _postModelBuilder;

        public PostController(
            IPostService postService,
            IContextService contextService)
        {
            _postService = postService;
            _contextService = contextService;
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
