namespace Lift.Core.Exception;

/// <summary>
/// 所有参数传递验证失败和Assert失败后抛出
/// </summary>
public class InvalidException : System.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public InvalidException() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public InvalidException(string message) : base(message) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public InvalidException(string message, System.Exception inner) : base(message, inner) { }
}
