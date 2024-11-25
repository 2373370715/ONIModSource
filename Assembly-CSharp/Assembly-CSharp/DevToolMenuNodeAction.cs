using System;

public class DevToolMenuNodeAction : IMenuNode
{
		public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

		public string GetName()
	{
		return this.name;
	}

		public void Draw()
	{
		if (ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
		{
			this.onClickFn();
		}
	}

		public string name;

		public System.Action onClickFn;

		public Func<bool> isEnabledFn;
}
