using System;
public static class EnumExtend
{
    public static Enum ToEnum<E>(this string enumString) where E : Enum
    {
        if (Enum.TryParse(typeof(E), enumString, out object e))
        {
            return (E)e;
        }
        else
        {
            return (E)Enum.Parse(typeof(E), "-1");
        }
    }
}