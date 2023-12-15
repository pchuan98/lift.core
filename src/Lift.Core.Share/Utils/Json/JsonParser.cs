using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lift.Core.Share.Utils;
using Lift.Core.Share.Utils.Json;

namespace Lift.Core.Utils.Json;

/**
 * todo docs
 *
 * 使用实现描述：使用过程中，应该通过Attribute来描述Json的解析方式
 *
 *1. 通过Attribute来描述Json的解析方式
 */


/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public static class JsonParser<T> where T : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool Read(ref T? obj, string? path = null)
    {
        try
        {
            if (Attribute.GetCustomAttribute(typeof(T), typeof(JsonAttribute)) is not JsonAttribute attr)
                return false;

            path ??= attr.Path;

            if (path is null) throw new InvalidException("The path must not null");

            obj = JsonSerializer.Deserialize<T>(File.ReadAllText(path));

            return true;
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message, e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public static bool Save(object obj, string? path = null)
    {
        try
        {
            if (Attribute.GetCustomAttribute(typeof(T), typeof(JsonAttribute)) is not JsonAttribute attr)
                return false;

            path ??= attr.Path;

            if (path is null) throw new InvalidException("The path must not null");

            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonCommonConverter<T>(),
                },

                WriteIndented = true
            });

            File.WriteAllText(path, json);

            return true;
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message, e);
        }

    }
}
