using System;
using System.Diagnostics;
using System.Reflection;

public static class AboutPrivate {
    /**
     * 获取私有字段的值
     */
    public static object GetPrivateValue<T>(T obj, string fieldName) where T : class {
        object result;
        try {
            var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                result = null;
            else
                result = field.GetValue(obj);
        } catch (Exception) { result = null; }

        return result;
    }

    // 获取静态私有字段的值
    public static object GetPrivateStaticValue<T>(T obj, string fieldName) where T : class {
        object result;
        try {
            var field = typeof(T).GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            if (field == null)
                result = null;
            else
                result = field.GetValue(obj);
        } catch (Exception) { result = null; }

        return result;
    }

    // 设置私有字段的值
    public static void SetPrivateValue<T>(T obj, string fieldName, object value) where T : class {
        try {
            var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (!(field == null)) field.SetValue(obj, value);
        } catch (Exception e) { throw e; }
    }

    /**
     * 调用私有方法
     */
    public static object CallPrivateMethod<T>(T obj, string methodName, object[] parameters = null) where T : class {
        object result;
        try {
            var method = typeof(T).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
                result = null;
            else {
                var obj2 = method.Invoke(obj, parameters);
                result = obj2;
            }
        } catch (Exception) { result = null; }

        return result;
    }
}