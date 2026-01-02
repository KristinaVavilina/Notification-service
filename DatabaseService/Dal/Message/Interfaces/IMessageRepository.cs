using Dal.Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Message.Interfaces;

public interface IMessageRepository
{
    public Task<Guid> CreateAsync(MessageDal dal);
    public Task<MessageDal?> GetAsync(Guid dal);
    public Task<Guid?> AddStatusAsync(MessageStatusDal dal);
}
