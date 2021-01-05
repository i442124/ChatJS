using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Models.Memberships;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatJS.WebServer.Controllers.Protected
{
    [Authorize]
    [ApiController]
    [Route("api/protected/memberships")]
    public class MembershipController : Controller
    {
        private readonly IMembershipRules _membershipRules;
        private readonly IMembershipService _membershipService;
        private readonly IMembershipModelBuilder _membershipModelBuilder;

        private readonly IContextService _contextService;
        private readonly INotificationService _notifyService;

        public MembershipController(
            IMembershipRules membershipRules,
            IMembershipService membershipService,
            IMembershipModelBuilder membershipModelBuilder,
            IContextService contextService,
            INotificationService notifyService)
        {
            _membershipRules = membershipRules;
            _membershipService = membershipService;
            _membershipModelBuilder = membershipModelBuilder;

            _contextService = contextService;
            _notifyService = notifyService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var user = await _contextService.CurrentUserAsync();
            return Ok(await _membershipModelBuilder.BuildMembershipPageModelAsync(user.Id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _membershipRules.IsValidAsync(user.Id, model.Chatroom.Id))
            {
                var command = new CreateMembership
                {
                    UserId = model.User.Id,
                    ChatroomId = model.Chatroom.Id,
                };

                await _membershipService.CreateAsync(command);

                var membershipModel = await _membershipModelBuilder
                .BuildMembershipModelAsync(command.ChatroomId);

                await _notifyService.SubscribeAsync(command.UserId, command.ChatroomId);
                await _notifyService.PublishAsync("CreateMembership", command.ChatroomId, membershipModel);

                return Ok(membershipModel);
            }

            return Unauthorized();
        }

        [HttpPost("suspend")]
        public async Task<IActionResult> SuspendAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _membershipRules.IsValidAsync(user.Id, model.Chatroom.Id))
            {
                var command = new SuspendMembership
                {
                    UserId = model.User.Id,
                    ChatroomId = model.Chatroom.Id
                };

                await _membershipService.SuspendAsync(command);

                var membership = new { command.ChatroomId, command.UserId };
                var membershipModel = await _membershipModelBuilder
                    .BuildMembershipModelAsync(membership.ChatroomId);

                await _notifyService.UnsubscribeAsync(command.UserId, command.ChatroomId);
                await _notifyService.PublishAsync("SuspendMembership", command.ChatroomId, membershipModel);

                return Ok(membershipModel);
            }

            return Unauthorized();
        }

        [HttpPost("reinstate")]
        public async Task<IActionResult> ReinstateAsync(FormComponentModel model)
        {
            var user = await _contextService.CurrentUserAsync();
            if (await _membershipRules.IsValidAsync(user.Id, model.Chatroom.Id))
            {
                var command = new ReinstateMembership
                {
                    UserId = model.User.Id,
                    ChatroomId = model.Chatroom.Id
                };

                await _membershipService.ReinstateAsync(command);

                var membership = new { command.ChatroomId, command.UserId };
                var membershipModel = await _membershipModelBuilder
                    .BuildMembershipModelAsync(membership.ChatroomId);

                await _notifyService.SubscribeAsync(command.UserId, command.ChatroomId);
                await _notifyService.PublishAsync("ReinstateMembership", command.ChatroomId, membershipModel);

                return Ok(membershipModel);
            }

            return Unauthorized();
        }
    }
}
