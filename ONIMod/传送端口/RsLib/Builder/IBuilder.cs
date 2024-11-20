using System.Collections.Generic;
using System.Text;

namespace RsLib.Builder;

public interface IBuilder {
    public bool AllowBuild(object obj);
    public void Build(object target, StringBuilder builder, string prefix, List<OtherBuilderInfo> otherBuilderInfos);
}