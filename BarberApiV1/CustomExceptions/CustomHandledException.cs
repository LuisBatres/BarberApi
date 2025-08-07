using Newtonsoft.Json;

namespace BarberApiV1.CustomExceptions;

public class CustomHandledException : Exception
{
    public string? Function { get; set; }
    public string? Class { get; set; }
    public object[]? FunctionArguments { get; set; }
    [JsonIgnore]
    public Exception? Exception { get; set; }
    public override string Message => Exception?.Message ?? "El mensaje de la excepcion fue nulo";
    public int? Line { get; set; }

    public CustomHandledException(Exception exception)
    {
        Exception = exception;
        Line = GetLineNumber(exception);
    }

    private static int? GetLineNumber(Exception exception)
    {
        var stackTrace = new System.Diagnostics.StackTrace(exception, true);
        var frame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
        return frame?.GetFileLineNumber();
    }
}