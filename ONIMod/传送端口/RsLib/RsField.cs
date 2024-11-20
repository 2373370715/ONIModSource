using System.Reflection;

namespace RsLib;

public class RsField {
    public static object GetValue(object target, string fieldName) {
        var field = target.GetType()
                          .GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        return field.GetValue(target);
    }

    public static void SetValue(object target, string fieldName, object value) {
        var field = target.GetType()
                          .GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        field.SetValue(target, value);
    }

    public static void Copy(object       source,
                            object       target,
                            BindingFlags targetBindingFlags = BindingFlags.Instance | BindingFlags.Public) {
        var fields = target.GetType().GetFields(targetBindingFlags);
        foreach (var field in fields) {
            var oldValue = GetValue(source, field.Name);
            field.SetValue(target, oldValue);
        }
    }
}