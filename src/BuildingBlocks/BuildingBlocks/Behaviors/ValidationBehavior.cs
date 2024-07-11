
using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse> (IEnumerable<IValidator<TRequest>> validators)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommand<TResponse>

{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);
		var validationRes = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));

		var failures = validationRes.Where(x => x.Errors.Any()).SelectMany(a => a.Errors).ToList();

		if (failures.Any()) throw new ValidationException(failures);
		return await next();
	}
}
