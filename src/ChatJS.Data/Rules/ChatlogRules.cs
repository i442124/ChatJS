using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class ChatlogRules : IChatlogRules
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatlogRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
