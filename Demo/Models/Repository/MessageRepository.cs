using Demo.Infrastructure;
using Demo.Models.Entities;
using Demo.Models.Interfaces;

namespace Demo.Models.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _appDbContext;
        public MessageRepository(AppDbContext appDbContext) { 
            _appDbContext = appDbContext;
        }
        public async Task InsertAsync(Message message)
        {
            await _appDbContext.Messages.AddAsync(message);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessagesAsync() =>

            _appDbContext.Messages.ToList();
    }
}
