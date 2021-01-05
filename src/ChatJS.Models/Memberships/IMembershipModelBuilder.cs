using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Memberships
{
    public interface IMembershipModelBuilder
    {
        public Task<MembershipPageModel> BuildMembershipPageModelAsync(Guid userId);

        public Task<MembershipPageModel.ChatroomModel> BuildMembershipModelAsync(Guid chatroomId);
    }
}
