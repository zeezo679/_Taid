using Web.Models.Entities;

namespace Core.Models.Interfaces.Messages
{
    public interface IMessageRepository
    {
        Task InsertAsync(Message message);
        Task<List<Message>> GetMessagesAsync();
        Task SaveChangesAsync();
    }
}
