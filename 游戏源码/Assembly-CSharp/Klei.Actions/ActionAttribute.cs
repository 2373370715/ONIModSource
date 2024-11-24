using System;

namespace Klei.Actions;

[AttributeUsage(AttributeTargets.Class)]
public class ActionAttribute : Attribute
{
	public readonly string ActionName;

	public ActionAttribute(string actionName)
	{
		ActionName = actionName;
	}
}
