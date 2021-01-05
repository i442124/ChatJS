using System;
using System.Data;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Services
{
    public class MessageService : IMessageService
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateMessage> _createValidator;
        private readonly IValidator<UpdateMessage> _updateValidator;

        public MessageService(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext,
            IValidator<CreateMessage> createValidator,
            IValidator<UpdateMessage> updateValidator)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task CreateAsync(CreateMessage command)
        {
            await _createValidator.ValidateAndThrowAsync(command);

            var message = new Message
            {
                Attachment = command.Attachment,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = command.UserId,
                Content = command.Content,
                Id = command.Id,
                Status = MessageStatusType.Published
            };

            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteMessage command)
        {
            var messageById = new GetMessageById { Id = command.Id };
            var message = await GetByIdAsync(messageById);

            message.Status = MessageStatusType.Deleted;
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.Message(message.Id));
        }

        public async Task<Message> GetByIdAsync(GetMessageById command)
        {
            var message = await _dbContext.Messages
                .FirstOrDefaultAsync(message =>
                    message.Id == command.Id &&
                    message.Status != MessageStatusType.Deleted);

            if (message == null)
            {
                throw new DataException($"Message with id {command.Id} not found.");
            }

            return message;
        }

        public async Task UpdateAsync(UpdateMessage command)
        {
            await _updateValidator.ValidateAndThrowAsync(command);

            var mesasgeById = new GetMessageById { Id = command.Id };
            var message = await GetByIdAsync(mesasgeById);

            message.Attachment = command.Attachment;
            message.Content = command.Content;
            message.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.Message(message.Id));
        }
    }
}
