using System;
using UnityEngine;

// Token: 0x02000F04 RID: 3844
public class OrnamentReceptacle : SingleEntityReceptacle
{
	// Token: 0x06004D92 RID: 19858 RVA: 0x000D260C File Offset: 0x000D080C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x000D2614 File Offset: 0x000D0814
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapTo_ornament", false);
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x00265604 File Offset: 0x00263804
	protected override void PositionOccupyingObject()
	{
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.transform.SetLocalPosition(new Vector3(0f, 0f, -0.1f));
		this.occupyingTracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
		this.occupyingTracker.symbol = new HashedString("snapTo_ornament");
		this.occupyingTracker.forceAlwaysVisible = true;
		this.animLink = new KAnimLink(base.GetComponent<KBatchedAnimController>(), component);
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x00265684 File Offset: 0x00263884
	protected override void ClearOccupant()
	{
		if (this.occupyingTracker != null)
		{
			UnityEngine.Object.Destroy(this.occupyingTracker);
			this.occupyingTracker = null;
		}
		if (this.animLink != null)
		{
			this.animLink.Unregister();
			this.animLink = null;
		}
		base.ClearOccupant();
	}

	// Token: 0x040035E1 RID: 13793
	[MyCmpReq]
	private SnapOn snapOn;

	// Token: 0x040035E2 RID: 13794
	private KBatchedAnimTracker occupyingTracker;

	// Token: 0x040035E3 RID: 13795
	private KAnimLink animLink;
}
