using System;

namespace Klei.Actions {
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionTypeAttribute : Attribute {
        public readonly bool   GenerateConfig;
        public readonly string GroupName;
        public readonly string TypeName;

        public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true) {
            TypeName       = typeName;
            GroupName      = groupName;
            GenerateConfig = generateConfig;
        }

        public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs) {
            var flag  = Equals(lhs, null);
            var flag2 = Equals(rhs, null);
            if (flag || flag2) return flag == flag2;

            return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
        }

        public static   bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs) { return !(lhs == rhs); }
        public override bool Equals(object                   obj) { return base.Equals(obj); }
        public override int  GetHashCode()                        { return base.GetHashCode(); }
    }
}