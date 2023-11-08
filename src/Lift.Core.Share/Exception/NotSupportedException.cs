namespace Lift.Core.Exception;

/// <summary>
/// 当前功能还没实现，但是必须实现的错误
/// </summary>
public class NotSupportedException : System.Exception
{
    public NotSupportedException() { }

    public NotSupportedException(string message) : base(message) { }

    public NotSupportedException(string message, System.Exception inner) : base(message, inner) { }
}
