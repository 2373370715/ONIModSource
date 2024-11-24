using System;

public class DevToolObjectViewer<T> : DevTool
{
	private Func<T> getValue;

	public DevToolObjectViewer(Func<T> getValue)
	{
		this.getValue = getValue;
		Name = typeof(T).Name;
	}

	protected override void RenderTo(DevPanel panel)
	{
		T val = getValue();
		Name = val.GetType().Name;
		ImGuiEx.DrawObject(val);
	}
}
