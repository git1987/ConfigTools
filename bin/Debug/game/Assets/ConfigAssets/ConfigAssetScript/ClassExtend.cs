using System;
using System.Collections.Generic;
using LitJson;
namespace ConfigTools
{
    public class vector2
    {
        public float x, y;
        public vector2()
        {
            x = y = 0;
        }
        public vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static implicit operator UnityEngine.Vector2(vector2 vector)
        {
            return UnityEngine.Vector2(vector.x, vector.y);
        }
        public static implicit operator UnityEngine.Vector3(vector2 vector)
        {
            return UnityEngine.Vector2(vector.x, vector.y, 0);
        }
    }
    public class vector3
    {
        public float x, y, z;
        public vector3()
        {
            x = y = z = 0;
        }
        public vector2(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator UnityEngine.Vector2(vector3 vector)
        {
            return UnityEngine.Vector2(vector.x, vector.y);
        }
        public static implicit operator UnityEngine.Vector3(vector3 vector)
        {
            return UnityEngine.Vector2(vector.x, vector.y, vector.z);
        }
    }
    public static class ClassExtend
    {
        public static List<T> ToList<T>(this object arrayList)
        {
            string str = arrayList as string;
            if (str == null) return new List<T>();
            if (str == string.Empty) return new List<T>();
            string[] strs = str.Split('|');
            List<T> list = new List<T>();
            for (int i = 0; i < strs.Length; i++)
            {
                T t = default(T);
                try
                {
                    t = (T)Convert.ChangeType(strs[i], typeof(T));
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogErrorFormat("[{0}] don't as [{1}]:{2}", str[i], default(T).GetType(), e.Message);
                    return new List<T>();
                }
                list.Add(t);
            }
            return list;
        }
        public static List<T> ToList<T>(this string str)
        {
            if (string.IsNullOrEmpty(str)) return new List<T>();
            List<T> list = new List<T>();
            string[] strs = str.Split('|');
            for (int i = 0; i < strs.Length; i++)
            {
                T t = default(T);
                try
                {
                    if (typeof(T).IsEnum)
                    {
                        if (strs[i] == string.Empty)
                        {
                            strs[i] = "None";
                        }
                        t = (T)System.Enum.Parse(typeof(T), strs[i]);
                    }
                    else
                    {
                        if (strs[i] == string.Empty)
                        {
                            if (typeof(T) == typeof(string)) { }
                            else if (typeof(T) == typeof(bool))
                            {
                                strs[i] = "false";
                            }
                            else if (typeof(T) == typeof(vector2))
                            {
                                strs[i] = new();
                            }
                            else if (typeof(T) == typeof(vector3))
                            {
                                strs[i] = new();
                            }
                            else
                            {
                                strs[i] = "0";
                            }
                        }
                        t = (T)Convert.ChangeType(strs[i], typeof(T));
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogErrorFormat("[{0}] don't as [{1}]:{2}", str, default(T).GetType(), e.Message);
                    return new List<T>();
                }
                list.Add(t);
            }
            return list;
        }
        public static vector2 Tovector2(this string str)
        {
            string[] vectorStr = str.Split(',');
            if (vectorStr.Length < 2)
            {
                UnityEngine.Debug.LogError("str length < 2");
                return new vector2();
            }
            else
            {
                int.TryParse(vectorStr[0], out int x);
                int.TryParse(vectorStr[1], out int y);
                return new vector2(x, y);
            }
        }
        public static vector3 Tovector3(this string str)
        {
            string[] vectorStr = str.Split(',');
            if (vectorStr.Length < 2)
            {
                UnityEngine.Debug.LogError("str length < 2");
                return new vector2();
            }
            else if (vectorStr.Length == 2)
            {
                int.TryParse(vectorStr[0], out int x);
                int.TryParse(vectorStr[1], out int y);
                return new vector3(x, y, 0);
            }
            else
            {
                int.TryParse(vectorStr[0], out int x);
                int.TryParse(vectorStr[1], out int y);
                int.TryParse(vectorStr[2], out int z);
                return new vector2(x, y, z);
            }
        }
        public static List<T> ToList<T>(this JsonData jsonData)
        {
            return jsonData.ToString().ToList<T>();
        }
        public static E ToEnum<E>(this string enumString) where E : Enum
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
}