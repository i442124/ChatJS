﻿using System;
using System.Threading.Tasks;

namespace ChatJS.Domain.Messages
{
    public interface IMessageRules
    {
        Task<bool> IsValidAsync(Guid messageId, Guid chatId);
    }
}