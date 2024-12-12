using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace WebShopApp.Models
{
    public static class ValidatedResultPresenter
    {
        public static ActionResult Present<TRepresentation>(Result<TRepresentation> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return PresentErrorResponse(result.Errors);
        }

        internal static ActionResult Present(Result result)
        {
            if (result.IsSuccess)
            {
                return new NoContentResult();
            }

            return PresentErrorResponse(result.Errors);
        }

        public static ActionResult PresentErrorResponse(List<IError> errors)
        {
            var errorResponse = new ErrorResponse
            {
                Errors = errors.Select(MapError).ToList(),
            };

            return new BadRequestObjectResult(errorResponse);
        }

        private static ErrorItem MapError(IError error)
        {
            var errorItem = new ErrorItem(error.Message);

            if (error.HasMetadataKey(ErrorWithReason.ReasonDescription) && error.HasMetadataKey(ErrorWithReason.ReasonName))
            {
                errorItem.Reason = new ErrorItemReason(
                    error.Metadata[ErrorWithReason.ReasonDescription]?.ToString() ?? string.Empty,
                    error.Metadata[ErrorWithReason.ReasonName]?.ToString() ?? string.Empty
                );
            }

            return errorItem;
        }
    }
}
