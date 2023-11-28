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
public class JsonParser<T> where T : class
{
    public T Object { get; set; }

    public JsonParser(T obj)
    {
        this.Object = obj;


        if (Attribute.GetCustomAttribute(typeof(T), typeof(JsonAttribute)) is not JsonAttribute attribute)
            throw new System.Exception();



    }


}
