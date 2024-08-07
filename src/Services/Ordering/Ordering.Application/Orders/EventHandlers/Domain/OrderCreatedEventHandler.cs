﻿using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, IPublishEndpoint publishEndpoint) 
	: INotificationHandler<OrderCreatedEvent>
{
	public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

		var orderCreatedIntegrationEvent = notification.order.ToOrderDto();

		await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
	}
}
