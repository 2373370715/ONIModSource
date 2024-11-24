public class DevToolAnimFile : DevTool
{
	private KAnimFile animFile;

	public DevToolAnimFile(KAnimFile animFile)
	{
		this.animFile = animFile;
		Name = "Anim File: \"" + animFile.name + "\"";
	}

	protected override void RenderTo(DevPanel panel)
	{
		ImGuiEx.DrawObject(animFile);
		ImGuiEx.DrawObject(animFile.GetData());
	}
}
