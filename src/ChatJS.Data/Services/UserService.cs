using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ChatJS.Data.Services
{
    public class UserService : IUserService
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateUser> _createValidator;
        private readonly IValidator<UpdateUser> _updateValidator;

        public UserService(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext,
            IValidator<CreateUser> createValidator,
            IValidator<UpdateUser> updateValidator)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task ConfirmAsync(ConfirmUser command)
        {
            var userById = new GetUserById { Id = command.Id };
            var user = await GetByIdAsync(userById);

            if (user.Status != UserStatusType.Pending)
            {
                throw new DataException("User is not in a pending state.");
            }

            user.Status = UserStatusType.Active;
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.User(user.Id));
            _cacheManager.Remove(CacheKeyCollection.Users(Guid.Empty));
        }

        public async Task CreateAsync(CreateUser command)
        {
            await _createValidator.ValidateAndThrowAsync(command);

            var user = new User
            {
                Status = UserStatusType.Pending,
                DisplayName = command.DisplayName,
                DisplayNameUid = command.DisplayNameUid,
                IdentityUserId = command.IdentityUserId,
                Id = command.Id
            };

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.Users(Guid.Empty));
        }

        public async Task DeleteAsync(DeleteUser command)
        {
            var userById = new GetUserById { Id = command.Id };
            var user = await GetByIdAsync(userById);

            user.Status = UserStatusType.Deleted;
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.User(user.Id));
            _cacheManager.Remove(CacheKeyCollection.Users(Guid.Empty));
        }

        public async Task<User> GetByIdAsync(GetUserById command)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(user =>
                    user.Id == command.Id &&
                    user.Status != UserStatusType.Deleted);

            if (user == null)
            {
                throw new DataException($"User with id {command.Id} not found.");
            }

            return user;
        }

        public async Task<User> GetByNameAsync(GetUserByName command)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(user =>
                    user.DisplayName == command.DisplayName &&
                    user.DisplayNameUid == command.DisplayNameUid &&
                    user.Status != UserStatusType.Deleted);

            if (user == null)
            {
                throw new DataException($"User '{command.DisplayName}#{command.DisplayNameUid}' not found.");
            }

            return user;
        }

        public async Task ReinstateAsync(ReinstateUser command)
        {
            var userById = new GetUserById { Id = command.Id };
            var user = await GetByIdAsync(userById);

            user.Status = UserStatusType.Active;
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.User(user.Id));
            _cacheManager.Remove(CacheKeyCollection.Users(Guid.Empty));
        }

        public async Task SuspendAsync(SuspendUser command)
        {
            var userById = new GetUserById { Id = command.Id };
            var user = await GetByIdAsync(userById);

            user.Status = UserStatusType.Suspended;
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.User(user.Id));
            _cacheManager.Remove(CacheKeyCollection.Users(Guid.Empty));
        }

        public async Task UpdateAsync(UpdateUser command)
        {
            await _updateValidator.ValidateAndThrowAsync(command);

            var userById = new GetUserById { Id = command.Id };
            var user = await GetByIdAsync(userById);

            user.DisplayName = command.DisplayName;
            user.DisplayNameUid = command.DisplayNameUid;

            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.User(user.Id));
            _cacheManager.Remove(CacheKeyCollection.Users(Guid.Empty));
        }
    }
}
