using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Data.Caching;
using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Posts;
using ChatJS.Models.Posts;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class PostModelBuilder : IPostModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public PostModelBuilder(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public Task<PostPageModel> BuildPostPageModelAsync(Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Posts(chatroomId), async () =>
            {
                var posts = await _dbContext.Posts
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.CreatedByUser)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .OrderBy(x => x.Message.CreatedAt)
                        .ToListAsync();

                return new PostPageModel
                {
                    Messages = posts.Select(post => _cacheManager.GetOrSet(
                    CacheKeyCollection.Post(post.Id), () =>
                    {
                        return new PostPageModel.MessageModel
                        {
                            Id = post.Message.Id,
                            Content = post.Message.Content,
                            TimeStamp = post.Message.CreatedAt,
                            Attachment = post.Message.Attachment,
                            Creator = new PostPageModel.UserModel
                            {
                                Id = post.Message.CreatedByUser.Id,
                                Name = post.Message.CreatedByUser.DisplayName,
                                NameUid = post.Message.CreatedByUser.DisplayNameUid
                            },
                            Delivery = new PostPageModel.DeliveryModel
                            {
                                IsReadByEveryone = post.Message.Deliveries.All(
                                x => x.Status == DeliveryStatusType.Read),

                                IsReceivedByEveryone = post.Message.Deliveries.All(
                                x => x.Status == DeliveryStatusType.Read ||
                                     x.Status == DeliveryStatusType.Received)
                            }
                        };
                    })).ToList()
                };
            });
        }

        public Task<PostPageModel.MessageModel> BuildMessageModelAsync(Guid postId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Post(postId), async () =>
            {
                var post = await _dbContext.Posts
                    .Where(x => x.Id == postId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.CreatedByUser)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .FirstOrDefaultAsync();

                return new PostPageModel.MessageModel
                {
                    Id = post.Message.Id,
                    Content = post.Message.Content,
                    TimeStamp = post.Message.CreatedAt,
                    Attachment = post.Message.Attachment,
                    Creator = new PostPageModel.UserModel
                    {
                        Id = post.Message.CreatedByUser.Id,
                        Name = post.Message.CreatedByUser.DisplayName,
                        NameUid = post.Message.CreatedByUser.DisplayNameUid
                    },
                    Delivery = new PostPageModel.DeliveryModel
                    {
                        IsReadByEveryone = post.Message.Deliveries.All(
                                x => x.Status == DeliveryStatusType.Read),

                        IsReceivedByEveryone = post.Message.Deliveries.All(
                                x => x.Status == DeliveryStatusType.Read ||
                                     x.Status == DeliveryStatusType.Received)
                    }
                };
            });
        }
    }
}
