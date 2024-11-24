using System;

namespace Klei.Actions;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ActionTypeAttribute : Attribute
{
	public readonly string TypeName;

	public readonly string GroupName;

	public readonly bool GenerateConfig;

	public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
	{
		TypeName = typeName;
		GroupName = groupName;
		GenerateConfig = generateConfig;
	}

	public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
	{
		bool flag = object.Equals(lhs, null);
		bool flag2 = object.Equals(rhs, null);
		if (flag || flag2)
		{
			return flag == flag2;
		}
		if (lhs.TypeName == rhs.TypeName)
		{
			return lhs.GroupName == rhs.GroupName;
		}
		return false;
	}

	public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
	{
		return !(lhs == rhs);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
