namespace Lift.Core.Exception;

/// <summary>
/// 所有参数传递验证失败和Assert失败后抛出
/// </summary>
public class InvalidException : System.Exception
{
    public InvalidException() { }

    public InvalidException(string message) : base(message) { }

    public InvalidException(string message, System.Exception inner) : base(message, inner) { }
}
