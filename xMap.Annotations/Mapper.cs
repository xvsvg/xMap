namespace xMap.MapperGenerators;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Mapper : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="result"></param>
    public Mapper(Type source, Type result)
    {
    }
}