using System;

public class DevToolMenuNodeAction : IMenuNode
{
	public string name;

	public System.Action onClickFn;

	public Func<bool> isEnabledFn;

	public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

	public string GetName()
	{
		return name;
	}

	public void Draw()
	{
		if (ImGuiEx.MenuItem(name, isEnabledFn == null || isEnabledFn()))
		{
			onClickFn();
		}
	}
}
