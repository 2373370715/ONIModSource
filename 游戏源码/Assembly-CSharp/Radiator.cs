using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200178E RID: 6030
[AddComponentMenu("KMonoBehaviour/scripts/Radiator")]
public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06007C0B RID: 31755 RVA: 0x0031EE3C File Offset: 0x0031D03C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.emitter = new RadiationGridEmitter(Grid.PosToCell(base.gameObject), this.intensity);
		this.emitter.projectionCount = this.projectionCount;
		this.emitter.direction = this.direction;
		this.emitter.angle = this.angle;
		if (base.GetComponent<Operational>() == null)
		{
			this.emitter.enabled = true;
		}
		else
		{
			base.Subscribe(824508782, new Action<object>(this.OnOperationalChanged));
		}
		RadiationGridManager.emitters.Add(this.emitter);
	}

	// Token: 0x06007C0C RID: 31756 RVA: 0x000F1B54 File Offset: 0x000EFD54
	protected override void OnCleanUp()
	{
		RadiationGridManager.emitters.Remove(this.emitter);
		base.OnCleanUp();
	}

	// Token: 0x06007C0D RID: 31757 RVA: 0x0031EEE4 File Offset: 0x0031D0E4
	private void OnOperationalChanged(object data)
	{
		bool isActive = base.GetComponent<Operational>().IsActive;
		this.emitter.enabled = isActive;
	}

	// Token: 0x06007C0E RID: 31758 RVA: 0x000F1B6D File Offset: 0x000EFD6D
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.intensity), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06007C0F RID: 31759 RVA: 0x000F1BA5 File Offset: 0x000EFDA5
	private void Update()
	{
		this.emitter.originCell = Grid.PosToCell(base.gameObject);
	}

	// Token: 0x04005DC7 RID: 24007
	public RadiationGridEmitter emitter;

	// Token: 0x04005DC8 RID: 24008
	public int intensity;

	// Token: 0x04005DC9 RID: 24009
	public int projectionCount;

	// Token: 0x04005DCA RID: 24010
	public int direction;

	// Token: 0x04005DCB RID: 24011
	public int angle = 360;
}
