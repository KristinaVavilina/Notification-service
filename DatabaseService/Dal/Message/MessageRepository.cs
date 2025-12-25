using Dal.Message.Interfaces;
using Dal.Message.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Message;

internal class MessageRepository : IMessageRepository
{
    private readonly MessageDbContext _dbContext;
    public MessageRepository(MessageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid?> AddStatusAsync(MessageStatusDal dal)
    {
        var message = await _dbContext.Messages.FindAsync(dal.MessageId);
        if (message is null)
        {
            return null;
        }
        message.StatusHistory.Add(dal);
        await _dbContext.SaveChangesAsync();
        return message.Id;
    }

    public async Task<Guid> CreateAsync(MessageDal dal)
    {
        await _dbContext.Messages.AddAsync(dal);
        await _dbContext.SaveChangesAsync();
        return dal.Id;
    }

    public async Task<MessageDal?> GetAsync(Guid id)
    {
        return await _dbContext.Messages
            .Include(x => x.Metadata)
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
