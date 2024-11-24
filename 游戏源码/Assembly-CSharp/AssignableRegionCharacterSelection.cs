using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001BFD RID: 7165
[AddComponentMenu("KMonoBehaviour/scripts/AssignableRegionCharacterSelection")]
public class AssignableRegionCharacterSelection : KMonoBehaviour
{
	// Token: 0x14000025 RID: 37
	// (add) Token: 0x060094D8 RID: 38104 RVA: 0x00397140 File Offset: 0x00395340
	// (remove) Token: 0x060094D9 RID: 38105 RVA: 0x00397178 File Offset: 0x00395378
	public event Action<MinionIdentity> OnDuplicantSelected;

	// Token: 0x060094DA RID: 38106 RVA: 0x00100E59 File Offset: 0x000FF059
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonPool = new UIPool<KButton>(this.buttonPrefab);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060094DB RID: 38107 RVA: 0x003971B0 File Offset: 0x003953B0
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.buttonPool.ClearAll();
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			KButton btn = this.buttonPool.GetFreeElement(this.buttonParent, true);
			CrewPortrait componentInChildren = btn.GetComponentInChildren<CrewPortrait>();
			componentInChildren.SetIdentityObject(minionIdentity, true);
			this.portraitList.Add(componentInChildren);
			btn.ClearOnClick();
			btn.onClick += delegate()
			{
				this.SelectDuplicant(btn);
			};
			this.buttonIdentityMap.Add(btn, minionIdentity);
		}
	}

	// Token: 0x060094DC RID: 38108 RVA: 0x00100E7E File Offset: 0x000FF07E
	public void Close()
	{
		this.buttonPool.DestroyAllActive();
		this.buttonIdentityMap.Clear();
		this.portraitList.Clear();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060094DD RID: 38109 RVA: 0x00100EAD File Offset: 0x000FF0AD
	private void SelectDuplicant(KButton btn)
	{
		if (this.OnDuplicantSelected != null)
		{
			this.OnDuplicantSelected(this.buttonIdentityMap[btn]);
		}
		this.Close();
	}

	// Token: 0x0400735A RID: 29530
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x0400735B RID: 29531
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x0400735C RID: 29532
	private UIPool<KButton> buttonPool;

	// Token: 0x0400735D RID: 29533
	private Dictionary<KButton, MinionIdentity> buttonIdentityMap = new Dictionary<KButton, MinionIdentity>();

	// Token: 0x0400735E RID: 29534
	private List<CrewPortrait> portraitList = new List<CrewPortrait>();
}
