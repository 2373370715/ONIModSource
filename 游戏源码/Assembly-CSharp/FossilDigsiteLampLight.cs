using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001775 RID: 6005
public class FossilDigsiteLampLight : Light2D
{
	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06007B81 RID: 31617 RVA: 0x000F13CE File Offset: 0x000EF5CE
	// (set) Token: 0x06007B80 RID: 31616 RVA: 0x000F13C5 File Offset: 0x000EF5C5
	public bool independent { get; private set; }

	// Token: 0x06007B82 RID: 31618 RVA: 0x000F13D6 File Offset: 0x000EF5D6
	protected override void OnPrefabInit()
	{
		base.Subscribe<FossilDigsiteLampLight>(-592767678, FossilDigsiteLampLight.OnOperationalChangedDelegate);
		base.IntensityAnimation = 1f;
	}

	// Token: 0x06007B83 RID: 31619 RVA: 0x0031C5CC File Offset: 0x0031A7CC
	public void SetIndependentState(bool isIndependent, bool checkOperational = true)
	{
		this.independent = isIndependent;
		Operational component = base.GetComponent<Operational>();
		if (component != null && this.independent && checkOperational && base.enabled != component.IsOperational)
		{
			base.enabled = component.IsOperational;
		}
	}

	// Token: 0x06007B84 RID: 31620 RVA: 0x000F13F4 File Offset: 0x000EF5F4
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.independent || base.enabled)
		{
			return base.GetDescriptors(go);
		}
		return new List<Descriptor>();
	}

	// Token: 0x04005CA3 RID: 23715
	private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>(delegate(FossilDigsiteLampLight light, object data)
	{
		if (light.independent)
		{
			light.enabled = (bool)data;
		}
	});
}
