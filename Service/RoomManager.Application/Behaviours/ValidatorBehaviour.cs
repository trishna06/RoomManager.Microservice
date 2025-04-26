using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Utility.Helper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RoomManager.Domain.Exceptions;

namespace RoomManager.Application.Behaviours
{
    public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<ValidatorBehaviour<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehaviour(IValidator<TRequest>[] validators, ILogger<ValidatorBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            List<FluentValidation.Results.ValidationFailure> failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                List<string> errorMessages = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToList();

                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {Errors}",
                    typeName, request, string.Join("; ", errorMessages));

                throw new RoomManagerDomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}: {string.Join("; ", errorMessages)}",
                    new ValidationException("Validation exception", failures));
            }

            return await next();
        }
    }
}
