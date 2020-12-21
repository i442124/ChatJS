using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatJS.Data.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateMessage> _createValidator;
        private readonly IValidator<UpdateMessage> _updateValidator;

        public MessageService(
            ApplicationDbContext dbContext,
            IValidator<CreateMessage> createValidator,
            IValidator<UpdateMessage> updateValidator)
        {
            _dbContext = dbContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task CreateAsync(CreateMessage command)
        {
            var result = await _createValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var message = new Message
                {
                    ChatlogId = command.ChatlogId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = command.UserId,
                    Content = command.Content,
                    Status = MessageStatusType.Published
                };

                await _dbContext.Messages.AddAsync(message);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task DeleteAsync(DeleteMessage command)
        {
            var message = await _dbContext.Messages
                  .FirstOrDefaultAsync(message =>
                      message.Status != MessageStatusType.Deleted &&
                      message.ChatlogId == command.ChatlogId &&
                      message.Index == command.Index);

            if (message == null)
            {
                throw new DataException($"Message '{command.Index}' in '{command.ChatlogId} was not found.");
            }

            message.Status = MessageStatusType.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateMessage command)
        {
            var message = await _dbContext.Messages
                  .FirstOrDefaultAsync(message =>
                      message.Status != MessageStatusType.Deleted &&
                      message.ChatlogId == command.ChatlogId &&
                      message.Index == command.Index);

            if (message == null)
            {
                throw new DataException($"Message '{command.Index}' in '{command.ChatlogId} was not found.");
            }

            message.Content = command.Content;
            message.CreatedAt = DateTime.UtcNow;
            message.CreatedBy = command.UserId;

            await _dbContext.SaveChangesAsync();
        }
    }
}
