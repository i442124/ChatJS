using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Services
{
    public class ChatroomService : IChatroomService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateChatroom> _createValidator;
        private readonly IValidator<UpdateChatroom> _updateValidator;

        public ChatroomService(
            ApplicationDbContext dbContext,
            IValidator<CreateChatroom> createValidator,
            IValidator<UpdateChatroom> updateValidator)
        {
            _dbContext = dbContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task CreateAsync(CreateChatroom command)
        {
            await _createValidator.ValidateAndThrowAsync(command);

            var chatroom = new Chatroom
            {
                Id = command.Id,
                Name = command.Name,
                NameCaption = command.NameCaption,
                Status = ChatroomStatusType.Active
            };

            await _dbContext.AddAsync(chatroom);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteChatroom command)
        {
            var chatroomById = new GetChatroomById { Id = command.Id };
            var chatroom = await GetByIdAsync(chatroomById);

            chatroom.Status = ChatroomStatusType.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Chatroom> GetByIdAsync(GetChatroomById command)
        {
            var chatroom = await _dbContext.Chatrooms
                .FirstOrDefaultAsync(chatroom =>
                    chatroom.Id == command.Id &&
                    chatroom.Status != ChatroomStatusType.Deleted);

            if (chatroom == null)
            {
                throw new DataException($"Chatroom with id {command.Id} not found.");
            }

            return chatroom;
        }

        public async Task UpdateAsync(UpdateChatroom command)
        {
            await _updateValidator.ValidateAndThrowAsync(command);

            var chatroomById = new GetChatroomById { Id = command.Id };
            var chatroom = await GetByIdAsync(chatroomById);

            chatroom.Name = command.Name;
            chatroom.NameCaption = command.NameCaption;

            await _dbContext.SaveChangesAsync();
        }
    }
}
