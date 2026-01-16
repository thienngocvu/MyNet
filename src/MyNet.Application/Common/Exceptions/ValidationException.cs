using FluentValidation.Results;
using MyNet.Application.Common.Constants;

namespace MyNet.Application.Common.Exceptions
{
    public class ValidationException : BaseException
    {
        public IDictionary<string, string[]> Failures { get; } = new Dictionary<string, string[]>();

        public ValidationException(string? message = null,
                                      string? detailCode = null,
                                      IDictionary<string, object>? payload = null,
                                      Exception? innerException = null)
            : base(ExceptionErrorCodeConstant.ERROR_ENTITY_VALIDATION, detailCode, payload, innerException)
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(
            string? detailCode = null,
            IDictionary<string, object>? payload = null,
            Exception? innerException = null)
        : base(ExceptionErrorCodeConstant.ERROR_ENTITY_VALIDATION, detailCode, payload, innerException) { }

        public ValidationException(List<ValidationFailure> failures,
                                      string? message = null,
                                      string? detailCode = null,
                                      IDictionary<string, object>? payload = null,
                                      Exception? innerException = null)
           : base(message, detailCode, payload, innerException)
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }
    }
}
