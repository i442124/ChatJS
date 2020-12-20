using System;
using System.Threading.Tasks;

namespace ChatJS.Domain.Chatlogs
{
    public interface IChatlogRules
    {
        Task<bool> IsValidAsync(Guid chatlogId);
    }
}
