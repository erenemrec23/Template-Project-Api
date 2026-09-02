using FluentValidation;
using MediatR;
using QrAssignment.Domain.Exceptions;

namespace QrAssignment.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    { 
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        // Kuralları çalıştır
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Hataları topla ve grupla
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
            .ToDictionary(x => x.Key, x => x.Select(m => m).Distinct().ToArray());
         
        if (failures.Any())
        {
            throw new ValidationAppException(failures);
        }
         
        return await next();
    }
}