using System;

// Token: 0x02001AC2 RID: 6850
public class DebugOverlays : KScreen
{
	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06008F81 RID: 36737 RVA: 0x000FDC94 File Offset: 0x000FBE94
	// (set) Token: 0x06008F82 RID: 36738 RVA: 0x000FDC9B File Offset: 0x000FBE9B
	public static DebugOverlays instance { get; private set; }

	// Token: 0x06008F83 RID: 36739 RVA: 0x00376F90 File Offset: 0x00375190
	protected override void OnPrefabInit()
	{
		DebugOverlays.instance = this;
		KPopupMenu componentInChildren = base.GetComponentInChildren<KPopupMenu>();
		componentInChildren.SetOptions(new string[]
		{
			"None",
			"Rooms",
			"Lighting",
			"Style",
			"Flow"
		});
		KPopupMenu kpopupMenu = componentInChildren;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelect));
		base.gameObject.SetActive(false);
	}

	// Token: 0x06008F84 RID: 36740 RVA: 0x0037700C File Offset: 0x0037520C
	private void OnSelect(string str, int index)
	{
		if (str == "None")
		{
			SimDebugView.Instance.SetMode(OverlayModes.None.ID);
			return;
		}
		if (str == "Flow")
		{
			SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
			return;
		}
		if (str == "Lighting")
		{
			SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
			return;
		}
		if (!(str == "Rooms"))
		{
			Debug.LogError("Unknown debug view: " + str);
			return;
		}
		SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
	}
}
