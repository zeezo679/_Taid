using Demo.Models.Entities;

namespace Demo.Models.Interfaces
{
    public interface IMessageQueue
    {
        void Enqueue(Message message);
        bool TryDequeue(out Message message);
    }
}
