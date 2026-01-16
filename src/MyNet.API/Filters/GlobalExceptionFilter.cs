using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyNet.Application.Common;
using MyNet.Application.Common.Exceptions;
using static MyNet.Application.Common.Constants.AppConstant;

namespace MyNet.API.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(System.ComponentModel.DataAnnotations.ValidationException), HandleValidationException },
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(BadRequestException), HandleBadRequestException},
                { typeof(DbUpdateException), HandleDbException},
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            context.Result = new ObjectResult(BaseResponse.Error(context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.SERVE_RERROR
            };

            _logger.LogError($"EXCEPTION {nameof(HandleUnknownException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleDbException(ExceptionContext context)
        {
            context.Result = new ObjectResult(BaseResponse.Error(context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.SERVE_RERROR
            };

            _logger.LogError($"EXCEPTION {nameof(HandleUnknownException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            context.Result = new ObjectResult(BaseResponse.Error(exception?.DetailCode ?? context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.VALIDATION
            };

            _logger.LogError($"EXCEPTION {nameof(HandleValidationException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            context.Result = new ObjectResult(BaseResponse.Error(context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.VALIDATION
            };

            _logger.LogError($"EXCEPTION {nameof(HandleInvalidModelStateException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result = new ObjectResult(BaseResponse.Error(exception?.DetailCode ?? context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.BAD_REQUEST
            };

            _logger.LogError($"EXCEPTION {nameof(HandleNotFoundException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;

            context.Result = new ObjectResult(BaseResponse.Error(exception?.DetailCode ?? context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.BAD_REQUEST
            };

            _logger.LogError($"EXCEPTION {nameof(HandleBadRequestException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleRequiredTokenException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;

            context.Result = new ObjectResult(BaseResponse.Error(context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.UNAUTHORIZED
            };

            _logger.LogError($"EXCEPTION {nameof(HandleRequiredTokenException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }

        private void HandleUnpermissionException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;

            context.Result = new ObjectResult(BaseResponse.Error(exception?.DetailCode ?? context.Exception.Message))
            {
                StatusCode = StatusCodeResponse.UNAUTHORIZED
            };

            _logger.LogError($"EXCEPTION {nameof(HandleUnpermissionException)}: {context.Exception.Message}");
            context.ExceptionHandled = true;
        }
    }
}
