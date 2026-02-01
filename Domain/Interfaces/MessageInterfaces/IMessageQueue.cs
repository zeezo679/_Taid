using Web.Models.Entities;

namespace Web.Models.Interfaces
{
    public interface IMessageQueue
    {
        void Enqueue(Message message);
        bool TryDequeue(out Message message);
    }
}
