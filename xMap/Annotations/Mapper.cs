namespace xMap.Annotations;

[AttributeUsage(AttributeTargets.Class)]
public class Mapper : Attribute
{
    public Mapper(Type source, Type result)
    {
    }
}