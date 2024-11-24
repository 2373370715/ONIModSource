using System;
using UnityEngine;

// Token: 0x02001CD4 RID: 7380
public class FoldOutPanel : KMonoBehaviour
{
	// Token: 0x06009A15 RID: 39445 RVA: 0x00104442 File Offset: 0x00102642
	protected override void OnSpawn()
	{
		MultiToggle componentInChildren = base.GetComponentInChildren<MultiToggle>();
		componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.OnClick));
		this.ToggleOpen(this.startOpen);
	}

	// Token: 0x06009A16 RID: 39446 RVA: 0x00104477 File Offset: 0x00102677
	private void OnClick()
	{
		this.ToggleOpen(!this.panelOpen);
	}

	// Token: 0x06009A17 RID: 39447 RVA: 0x00104488 File Offset: 0x00102688
	private void ToggleOpen(bool open)
	{
		this.panelOpen = open;
		this.container.SetActive(this.panelOpen);
		base.GetComponentInChildren<MultiToggle>().ChangeState(this.panelOpen ? 1 : 0);
	}

	// Token: 0x04007849 RID: 30793
	private bool panelOpen = true;

	// Token: 0x0400784A RID: 30794
	public GameObject container;

	// Token: 0x0400784B RID: 30795
	public bool startOpen = true;
}
