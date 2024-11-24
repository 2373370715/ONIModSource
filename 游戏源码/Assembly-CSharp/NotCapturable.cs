using System;
using UnityEngine;

// Token: 0x020011B8 RID: 4536
[AddComponentMenu("KMonoBehaviour/scripts/NotCapturable")]
public class NotCapturable : KMonoBehaviour
{
	// Token: 0x06005C80 RID: 23680 RVA: 0x000DC5E2 File Offset: 0x000DA7E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (base.GetComponent<Capturable>() != null)
		{
			DebugUtil.LogErrorArgs(this, new object[]
			{
				"Entity has both Capturable and NotCapturable!"
			});
		}
		Components.NotCapturables.Add(this);
	}

	// Token: 0x06005C81 RID: 23681 RVA: 0x000DC617 File Offset: 0x000DA817
	protected override void OnCleanUp()
	{
		Components.NotCapturables.Remove(this);
		base.OnCleanUp();
	}
}
