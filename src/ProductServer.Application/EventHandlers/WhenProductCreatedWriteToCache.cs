using MediatR;
using Microsoft.Extensions.Logging;
using ProductServer.Application.Services.LastCreatedProductService;
using ProductServer.Domain.Aggregates.Product.DomainEvents;

namespace ProductServer.Application.EventHandlers
{
    internal class WhenProductCreatedWriteToCache : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ILogger<WhenProductCreatedWriteToCache> logger;
        private readonly ILastCreatedProductService lastCreatedProductService;

        public WhenProductCreatedWriteToCache(ILogger<WhenProductCreatedWriteToCache> logger, ILastCreatedProductService lastCreatedProductService)
        {
            this.logger = logger;
            this.lastCreatedProductService = lastCreatedProductService;
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            LogEventHandling(notification);

            try
            {
                await lastCreatedProductService.WriteToCacheAtomicallyAsync(new(notification.Id, notification.Denomination), cancellationToken).ConfigureAwait(false);

                LogEventHandledSuccessfully(notification);
            }
            catch (Exception ex)
            {
                LogEventException(ex, notification);
            }
        }

        private void LogEventHandling(INotification notification)
        {
            logger.LogDebug("HANDLING EVENT {eventName}; HANDLER {handlerName}; PARAMETER: {@eventData}", 
                nameof(ProductCreatedEvent), 
                nameof(WhenProductCreatedWriteToCache), 
                notification);
        }
        private void LogEventHandledSuccessfully(INotification notification)
        {
            logger.LogInformation("EVENT {eventName} HANDLED SUCCESSFULLY; HANDLER {handlerName}; PARAMETER: {@eventData}", 
                nameof(ProductCreatedEvent), 
                nameof(WhenProductCreatedWriteToCache), 
                notification);
        }
        private void LogEventException(Exception exception, INotification notification)
        {
            logger.LogError(exception, "EVENT {eventName} HANDLING FAILED; HANDLER {handlerName}; PARAMETER: {@eventData}" + "{newLine}", 
                nameof(ProductCreatedEvent), 
                nameof(WhenProductCreatedWriteToCache), 
                notification, 
                Environment.NewLine);
        }
    }
}
