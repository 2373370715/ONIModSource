using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001DF3 RID: 7667
public class DiscoveredSpaceMessage : Message
{
	// Token: 0x0600A05E RID: 41054 RVA: 0x001082E7 File Offset: 0x001064E7
	public DiscoveredSpaceMessage()
	{
	}

	// Token: 0x0600A05F RID: 41055 RVA: 0x001083F9 File Offset: 0x001065F9
	public DiscoveredSpaceMessage(Vector3 pos)
	{
		this.cameraFocusPos = pos;
		this.cameraFocusPos.z = -40f;
	}

	// Token: 0x0600A060 RID: 41056 RVA: 0x00108418 File Offset: 0x00106618
	public override string GetSound()
	{
		return "Discover_Space";
	}

	// Token: 0x0600A061 RID: 41057 RVA: 0x0010841F File Offset: 0x0010661F
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.TOOLTIP;
	}

	// Token: 0x0600A062 RID: 41058 RVA: 0x0010842B File Offset: 0x0010662B
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.NAME;
	}

	// Token: 0x0600A063 RID: 41059 RVA: 0x000AD332 File Offset: 0x000AB532
	public override string GetTooltip()
	{
		return null;
	}

	// Token: 0x0600A064 RID: 41060 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x0600A065 RID: 41061 RVA: 0x00108437 File Offset: 0x00106637
	public override void OnClick()
	{
		this.OnDiscoveredSpaceClicked();
	}

	// Token: 0x0600A066 RID: 41062 RVA: 0x0010843F File Offset: 0x0010663F
	private void OnDiscoveredSpaceClicked()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound(this.GetSound(), false));
		MusicManager.instance.PlaySong("Stinger_Surface", false);
		CameraController.Instance.SetTargetPos(this.cameraFocusPos, 8f, true);
	}

	// Token: 0x04007D71 RID: 32113
	[Serialize]
	private Vector3 cameraFocusPos;

	// Token: 0x04007D72 RID: 32114
	private const string MUSIC_STINGER = "Stinger_Surface";
}
