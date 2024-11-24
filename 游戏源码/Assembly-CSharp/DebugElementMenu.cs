using System;
using UnityEngine;

// Token: 0x02001C97 RID: 7319
public class DebugElementMenu : KButtonMenu
{
	// Token: 0x060098A5 RID: 39077 RVA: 0x00103513 File Offset: 0x00101713
	protected override void OnPrefabInit()
	{
		DebugElementMenu.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060098A6 RID: 39078 RVA: 0x00103528 File Offset: 0x00101728
	protected override void OnForcedCleanUp()
	{
		DebugElementMenu.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060098A7 RID: 39079 RVA: 0x00103536 File Offset: 0x00101736
	public void Turnoff()
	{
		this.root.gameObject.SetActive(false);
	}

	// Token: 0x040076E1 RID: 30433
	public static DebugElementMenu Instance;

	// Token: 0x040076E2 RID: 30434
	public GameObject root;
}
