using FluentResults;

namespace WebShopApp.Models
{
    public class ErrorResponse
    {
        public List<ErrorItem> Errors { get; set; } = new List<ErrorItem>();
    }

    public class ErrorItem
    {
        public ErrorItem(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        public ErrorItemReason? Reason { get; set; }
    }

    public class ErrorItemReason
    {
        public ErrorItemReason(string description, string name)
        {
            Description = description;
            Name = name;
        }

        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class ErrorWithReason : Error
    {
        public static readonly string ReasonDescription = "ReasonDescription";
        public static readonly string ReasonName = "ReasonName";

        public ErrorWithReason(string message) : base(message)
        {
        }

        public ErrorWithReason WithReason(string description, string name)
        {
            Metadata.Add(ReasonDescription, description);
            Metadata.Add(ReasonName, name);
            return this;
        }
    }
}
