using System;
using UnityEngine;

// Token: 0x0200123D RID: 4669
public struct DiseaseContainer
{
	// Token: 0x06005F9A RID: 24474 RVA: 0x002AACB4 File Offset: 0x002A8EB4
	public DiseaseContainer(GameObject go, ushort elemIdx)
	{
		this.elemIdx = elemIdx;
		this.isContainer = (go.GetComponent<IUserControlledCapacity>() != null && go.GetComponent<Storage>() != null);
		Conduit component = go.GetComponent<Conduit>();
		if (component != null)
		{
			this.conduitType = component.type;
		}
		else
		{
			this.conduitType = ConduitType.None;
		}
		this.controller = go.GetComponent<KBatchedAnimController>();
		this.overpopulationCount = 1;
		this.instanceGrowthRate = 1f;
		this.accumulatedError = 0f;
		this.visualDiseaseProvider = null;
		this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
		if (this.autoDisinfectable != null)
		{
			AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
		}
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x000DE51F File Offset: 0x000DC71F
	public void Clear()
	{
		this.controller = null;
	}

	// Token: 0x040043CF RID: 17359
	public AutoDisinfectable autoDisinfectable;

	// Token: 0x040043D0 RID: 17360
	public ushort elemIdx;

	// Token: 0x040043D1 RID: 17361
	public bool isContainer;

	// Token: 0x040043D2 RID: 17362
	public ConduitType conduitType;

	// Token: 0x040043D3 RID: 17363
	public KBatchedAnimController controller;

	// Token: 0x040043D4 RID: 17364
	public GameObject visualDiseaseProvider;

	// Token: 0x040043D5 RID: 17365
	public int overpopulationCount;

	// Token: 0x040043D6 RID: 17366
	public float instanceGrowthRate;

	// Token: 0x040043D7 RID: 17367
	public float accumulatedError;
}
