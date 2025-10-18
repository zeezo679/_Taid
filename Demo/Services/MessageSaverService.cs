
using Demo.Infrastructure;
using Demo.Models.Interfaces;

namespace Demo.Services
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
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    await db.Messages.AddAsync(message, stoppingToken);
                    await db.SaveChangesAsync(stoppingToken);
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
