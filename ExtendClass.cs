namespace ConfigTools
{
    internal static class ExtendClass
    {
        public static int? ToIntOrNull(this string str)
        {
            try
            {
                int i = int.Parse(str);
                return i;
            }
            catch
            {
                return null;
            }
        }

        #region string
        public static int ToInt(this string str, int defaultInt = 0)
        {
            if (int.TryParse(str, out int i))
            {
                return i;
            }
            return defaultInt;
        }
        public static float ToFloat(this string str, float defaultFloat = 0)
        {
            if (float.TryParse(str, out float f))
            {
                return f;
            }
            return defaultFloat;
        }
        public static long ToLong(this string str, long defaultLong = 0)
        {
            if (long.TryParse(str, out long l))
            {
                return l;
            }
            return defaultLong;
        }
        #endregion
        #region List
        /// <summary>
        /// 遍历List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">传参list中的item的回调</param>
        public static void ForAction<T>(this List<T> list, EventAction<T> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action?.Invoke(list[i]);
            }
        }
        /// <summary>
        /// 遍历List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">传参list中的item和index的回调</param>
        public static void ForAction<T>(this List<T> list, EventAction<T, int> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action?.Invoke(list[i], i);
            }
        }
        /// <summary>
        /// 遍历List：通过breakFunc返回值委托判断是否从循环遍历中跳出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">传参list中的item的回调</param>
        /// <param name="breakFunc">判断是否从遍历中break的bool返回值委托</param>
        public static void ForAction<T>(this List<T> list, EventAction<T> action, EventFunction<bool> breakFunc)
        {
            if (breakFunc == null)
                list.ForAction(action);
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    action?.Invoke(list[i]);
                    if (breakFunc.Invoke()) break;
                }
            }
        }
        /// <summary>
        /// 遍历List：通过breakFunc返回值委托判断是否从循环遍历中跳出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action">传参list中的item和index的回调</param>
        /// <param name="breakFunc">判断是否从遍历中break的bool返回值委托</param>
        public static void ForAction<T>(this List<T> list, EventAction<T, int> action, EventFunction<bool> breakFunc)
        {
            if (breakFunc == null)
                list.ForAction(action);
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    action?.Invoke(list[i], i);
                    if (breakFunc.Invoke()) break;
                }
            }
        }
        /// <summary>
        /// 遍历List：通过isBreak变量判断是否从循环遍历中跳出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <param name="isBreak">满足break条件的bool变量</param>
        public static void ForAction<T>(this List<T> list, EventAction<T> action, in bool isBreak)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action?.Invoke(list[i]);
                if (isBreak) break;
            }
        }
        /// <summary>
        /// 遍历List：通过isBreak变量判断是否冲循环遍历中跳出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <param name="isBreak">满足break条件的bool变量</param>
        public static void ForAction<T>(this List<T> list, EventAction<T, int> action, in bool isBreak)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action?.Invoke(list[i], i);
                if (isBreak) break;
            }
        }
        #endregion
        #region Dictionary
        /// <summary>
        /// 遍历Dictionary
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="action"></param>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        public static void ForAction<K, V>(this Dictionary<K, V> dic, EventAction<K, V> action)
        {
            List<K> kList = new List<K>(dic.Keys);
            List<V> vList = new List<V>(dic.Values);
            for (int i = 0; i < kList.Count; i++)
            {
                action?.Invoke(kList[i], vList[i]);
            }
        }
        /// <summary>
        /// 遍历Dictionary：通过breakAction回调判断是否冲循环遍历中跳出
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="action"></param>
        /// <param name="breakAction">跳出遍历回调</param>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        public static void ForAction<K, V>(this Dictionary<K, V> dic, EventAction<K, V> action,
                                           EventFunction<bool> breakAction)
        {
            if (breakAction == null)
            {
                dic.ForAction(action);
            }
            else
            {
                List<K> kList = new List<K>(dic.Keys);
                List<V> vList = new List<V>(dic.Values);
                for (int i = 0; i < kList.Count; i++)
                {
                    action?.Invoke(kList[i], vList[i]);
                    if (breakAction.Invoke()) break;
                }
            }
        }
        /// <summary>
        /// 遍历Dictionary：通过isBreak变量判断是否冲循环遍历中跳出
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="action"></param>
        /// <param name="isBreak">跳出遍历</param>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        public static void ForAction<K, V>(this Dictionary<K, V> dic, EventAction<K, V> action, in bool isBreak)
        {
            List<K> kList = new List<K>(dic.Keys);
            List<V> vList = new List<V>(dic.Values);
            for (int i = 0; i < kList.Count; i++)
            {
                action?.Invoke(kList[i], vList[i]);
                if (isBreak) break;
            }
        }
        #endregion
    }
}
