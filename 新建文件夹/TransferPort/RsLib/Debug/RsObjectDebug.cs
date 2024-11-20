using System.Reflection;
using System.Text;

namespace RsLib;

public class RsObjectDebug {
    private static readonly StringBuilder stringBuilder = new();

    public static string BuildAllField(object obj) {
        var fieldInfos = obj.GetType()
                            .GetFields(BindingFlags.Instance  |
                                       BindingFlags.Static    |
                                       BindingFlags.Public    |
                                       BindingFlags.NonPublic |
                                       BindingFlags.FlattenHierarchy);

        stringBuilder.Clear();
        foreach (var info in fieldInfos)
            stringBuilder.AppendLine().Append(info.Name).Append(":").Append(info.GetValue(obj) ?? "<null>");

        return stringBuilder.ToString();
    }
}