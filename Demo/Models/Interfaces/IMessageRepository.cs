using Demo.Models.Entities;

namespace Demo.Models.Interfaces
{
    public interface IMessageRepository
    {
        Task InsertAsync(Message message);
        Task<List<Message>> GetMessagesAsync();
    }
}
