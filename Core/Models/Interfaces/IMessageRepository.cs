using Web.Models.Entities;

namespace Web.Models.Interfaces
{
    public interface IMessageRepository
    {
        Task InsertAsync(Message message);
        Task<List<Message>> GetMessagesAsync();
    }
}
