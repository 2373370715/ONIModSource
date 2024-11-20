using System.Collections.Generic;
using System.Text;

namespace RsLib.Builder;

public class BuilderManager : List<IBuilder> {
    private readonly StringBuilder stringBuilder = new();

    private List<IBuilder> FindAllowBuilder(object obj) {
        var list = new List<IBuilder>();
        foreach (var builder in this)
            if (builder.AllowBuild(obj))
                list.Add(builder);

        return list;
    }

    public void Build(object obj, string prefix = "") {
        var builderInfos = new List<OtherBuilderInfo>();
        foreach (var builder in this)
            if (builder.AllowBuild(obj))
                builder.Build(obj, stringBuilder, prefix, builderInfos);

        foreach (var builderInfo in builderInfos) {
            stringBuilder.AppendLine();
            Build(builderInfo.target, builderInfo.prefix);
        }
    }

    public string BuildToString(object obj, string prefix = "", bool newLine = false) {
        stringBuilder.Clear();
        if (newLine) stringBuilder.AppendLine();
        Build(obj, prefix);
        return stringBuilder.ToString();
    }
}