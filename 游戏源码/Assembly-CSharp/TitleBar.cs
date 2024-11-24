using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200202C RID: 8236
[AddComponentMenu("KMonoBehaviour/scripts/TitleBar")]
public class TitleBar : KMonoBehaviour
{
	// Token: 0x0600AF58 RID: 44888 RVA: 0x00111F59 File Offset: 0x00110159
	public void SetTitle(string Name)
	{
		this.titleText.text = Name;
	}

	// Token: 0x0600AF59 RID: 44889 RVA: 0x00111F67 File Offset: 0x00110167
	public void SetSubText(string subtext, string tooltip = "")
	{
		this.subtextText.text = subtext;
		this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
	}

	// Token: 0x0600AF5A RID: 44890 RVA: 0x00111F86 File Offset: 0x00110186
	public void SetWarningActve(bool state)
	{
		this.WarningNotification.SetActive(state);
	}

	// Token: 0x0600AF5B RID: 44891 RVA: 0x00111F94 File Offset: 0x00110194
	public void SetWarning(Sprite icon, string label)
	{
		this.SetWarningActve(true);
		this.NotificationIcon.sprite = icon;
		this.NotificationText.text = label;
	}

	// Token: 0x0600AF5C RID: 44892 RVA: 0x00111FB5 File Offset: 0x001101B5
	public void SetPortrait(GameObject target)
	{
		this.portrait.SetPortrait(target);
	}

	// Token: 0x04008A0C RID: 35340
	public LocText titleText;

	// Token: 0x04008A0D RID: 35341
	public LocText subtextText;

	// Token: 0x04008A0E RID: 35342
	public GameObject WarningNotification;

	// Token: 0x04008A0F RID: 35343
	public Text NotificationText;

	// Token: 0x04008A10 RID: 35344
	public Image NotificationIcon;

	// Token: 0x04008A11 RID: 35345
	public Sprite techIcon;

	// Token: 0x04008A12 RID: 35346
	public Sprite materialIcon;

	// Token: 0x04008A13 RID: 35347
	public TitleBarPortrait portrait;

	// Token: 0x04008A14 RID: 35348
	public bool userEditable;

	// Token: 0x04008A15 RID: 35349
	public bool setCameraControllerState = true;
}
