namespace Lift.Core.Utils.Json;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class JsonAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    public string? Path { get; set; } = null;

    /// <summary>
    /// 
    /// </summary>
    public bool IncludeAll { get; set; } = true;
}

/// <summary>
/// Int属性的数据
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class JsonIntAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    public int Default { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public int Min { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public int Max { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public bool Usage { get; set; } = false;
}

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class JsonExcludeAttribute : Attribute
{

}


