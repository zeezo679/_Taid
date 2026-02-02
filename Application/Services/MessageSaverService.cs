using Core.Models.Interfaces.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Models.Interfaces;

namespace Services
{
    public class MessageSaverService : BackgroundService
    {
        private readonly IMessageQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory; // to make instance of DB for that service alone
        public MessageSaverService(IMessageQueue queue, IServiceScopeFactory scopeFactory)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
        }

        //The cancellation token is sent by the Host / Application when the Application Lifetime ends
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested) //while there is something happening keep checking for the messages in the queue untill the web closes
            {
                if(_queue.TryDequeue(out var message)) //we use tryDeque for concurrency
                {
                    using var scope = _scopeFactory.CreateScope();
                    var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();

                    await messageRepository.InsertAsync(message);
                    await messageRepository.SaveChangesAsync();
                }
                else
                {
                    //wait and try again if the queue is empty.
                    await Task.Delay(50, stoppingToken);
                }
            }
        }
    }
}
