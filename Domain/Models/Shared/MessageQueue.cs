using System.Collections.Concurrent;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.Models
{
    public class MessageQueue : IMessageQueue
    {
        private readonly ConcurrentQueue<Message> _messages = new();
        public void Enqueue(Message message)
        {
            _messages.Enqueue(message);
        }

        public bool TryDequeue(out Message message)
        {
            return _messages.TryDequeue(out message);
        }
    }
}
