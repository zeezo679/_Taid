using Core.Models.Interfaces.Messages;
using Web.Infrastructure;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Models.Repository
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
