namespace Overwatch.FeatureFlag.Api.ErrorHandling;

internal sealed class ApiError(string message, string innerMessage, string stackTrace)
{
    public string Message { get; set; } = message;
    public string InnerMessage { get; set; } = innerMessage;
    public string StackTrace { get; set; } = stackTrace;
}