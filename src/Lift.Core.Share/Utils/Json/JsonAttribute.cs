namespace Lift.Core.Utils.Json;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class JsonAttribute : Attribute
{
    /// <summary>
    /// The default path for save and read json file path.
    /// When the path is null, the default path not exists.So you must deliver a value with .Save and .Read
    /// </summary>
    public string? Path { get; set; } = null;

    /// <summary>
    /// Wether to include all properties.
    /// </summary>
    public bool IncludeAll { get; set; } = true;
}

/// <summary>
/// When the property attach this attribute, the property will be ignored.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ExcludeAttribute : Attribute { }

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class IncludeAttribute : Attribute { }
