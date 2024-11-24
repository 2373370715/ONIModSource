using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B67 RID: 2919
[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
	// Token: 0x06003777 RID: 14199 RVA: 0x000AB71E File Offset: 0x000A991E
	private Unsealable()
	{
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x000C3F5E File Offset: 0x000C215E
	public override CellOffset[] GetOffsets(int cell)
	{
		if (this.facingRight)
		{
			return OffsetGroups.RightOnly;
		}
		return OffsetGroups.LeftOnly;
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x000C3F73 File Offset: 0x000C2173
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_door_poi_kanim")
		};
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x00217E78 File Offset: 0x00216078
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
		if (this.unsealed)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				component.allowDeconstruction = true;
			}
		}
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x000AB715 File Offset: 0x000A9915
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x00217EB8 File Offset: 0x002160B8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.unsealed = true;
		base.OnCompleteWork(worker);
		Deconstructable component = base.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.allowDeconstruction = true;
			Game.Instance.Trigger(1980521255, base.gameObject);
		}
	}

	// Token: 0x0400259C RID: 9628
	[Serialize]
	public bool facingRight;

	// Token: 0x0400259D RID: 9629
	[Serialize]
	public bool unsealed;
}
