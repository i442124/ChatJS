using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Chatlogs.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatJS.Data.Services
{
    public class ChatlogService : IChatlogService
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatlogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(CreateChatlog command)
        {
            await _dbContext.AddAsync(new Chatlog { Id = command.Id, Status = ChatlogStatusType.Published });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteChatlog command)
        {
            var chatlogById = new GetChatlogById { Id = command.Id };
            var chatlog = await GetByIdAsync(chatlogById);

            chatlog.Status = ChatlogStatusType.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Chatlog> GetByIdAsync(GetChatlogById command)
        {
            var chatlog = await _dbContext.Chatlogs
                .FirstOrDefaultAsync(chatlog =>
                    chatlog.Id == command.Id &&
                    chatlog.Status != ChatlogStatusType.Deleted);

            if (chatlog == null)
            {
                throw new DataException($"Chatlog with Id '{command.Id}' was not found");
            }

            return chatlog;
        }
    }
}
