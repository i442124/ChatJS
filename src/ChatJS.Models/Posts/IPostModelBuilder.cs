using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Posts
{
    public interface IPostModelBuilder
    {
        Task<PostPageModel> BuildPostPageModelAsync(Guid chatroomId);

        Task<PostPageModel.MessageModel> BuildMessageModelAsync(Guid postId);
    }
}
