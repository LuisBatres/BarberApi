namespace BarberApiV1.Models;

public class CustomHandledExceptionResponse
{
    public string? Function { get; set; } = "";
    
    public string? Class { get; set;} = "";

    public object[]? FunctionArguments { get; set; } = null;
    
    public string? Message { get; set;} = "";
    
    public int? Line { get; set;} = 0;
}