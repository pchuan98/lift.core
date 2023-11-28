using System;
using System.Collections.Generic;
using System.Text;

namespace Lift.Core.Share.Exception;

/// <summary>
/// Will not implement this possibility, but the error occurred.
/// </summary>
public class NeverSupportedException : System.Exception
{
    /// <summary>
    /// <inheritdoc cref="NeverSupportedException"/>
    /// </summary>
    public NeverSupportedException() { }

    /// <summary>
    /// <inheritdoc cref="NeverSupportedException"/>
    /// </summary>
    public NeverSupportedException(string message) : base(message) { }

    /// <summary>
    /// <inheritdoc cref="NeverSupportedException"/>
    /// </summary>
    public NeverSupportedException(string message, System.Exception inner) : base(message, inner) { }
}
