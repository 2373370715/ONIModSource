using System;
using UnityEngine;

// Token: 0x02001C14 RID: 7188
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
	// Token: 0x0600956D RID: 38253 RVA: 0x00101508 File Offset: 0x000FF708
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x0600956E RID: 38254 RVA: 0x00101516 File Offset: 0x000FF716
	public void Register()
	{
		if (this.registered)
		{
			return;
		}
		this.registered = true;
		NameDisplayScreen.Instance.AddNewEntry(base.gameObject);
	}

	// Token: 0x04007403 RID: 29699
	public bool shouldShowName;

	// Token: 0x04007404 RID: 29700
	private bool registered;
}
