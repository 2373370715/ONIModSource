using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020013EF RID: 5103
public class HighEnergyParticlePort : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060068BE RID: 26814 RVA: 0x000E4A73 File Offset: 0x000E2C73
	public int GetHighEnergyParticleInputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleInputCell();
	}

	// Token: 0x060068BF RID: 26815 RVA: 0x000E4A80 File Offset: 0x000E2C80
	public int GetHighEnergyParticleOutputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleOutputCell();
	}

	// Token: 0x060068C0 RID: 26816 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060068C1 RID: 26817 RVA: 0x000E4A8D File Offset: 0x000E2C8D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticlePorts.Add(this);
	}

	// Token: 0x060068C2 RID: 26818 RVA: 0x000E4AA0 File Offset: 0x000E2CA0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HighEnergyParticlePorts.Remove(this);
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x002D8404 File Offset: 0x002D6604
	public bool InputActive()
	{
		Operational component = base.GetComponent<Operational>();
		return this.particleInputEnabled && component != null && component.IsFunctional && (!this.requireOperational || component.IsOperational);
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x000E4AB3 File Offset: 0x000E2CB3
	public bool AllowCapture(HighEnergyParticle particle)
	{
		return this.onParticleCaptureAllowed == null || this.onParticleCaptureAllowed(particle);
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x000E4ACB File Offset: 0x000E2CCB
	public void Capture(HighEnergyParticle particle)
	{
		this.currentParticle = particle;
		if (this.onParticleCapture != null)
		{
			this.onParticleCapture(particle);
		}
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x000E4AE8 File Offset: 0x000E2CE8
	public void Uncapture(HighEnergyParticle particle)
	{
		if (this.onParticleUncapture != null)
		{
			this.onParticleUncapture(particle);
		}
		this.currentParticle = null;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x002D8444 File Offset: 0x002D6644
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.particleInputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_INPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_INPUT, Descriptor.DescriptorType.Requirement, false));
		}
		if (this.particleOutputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_OUTPUT, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04004F09 RID: 20233
	[MyCmpGet]
	private Building m_building;

	// Token: 0x04004F0A RID: 20234
	public HighEnergyParticlePort.OnParticleCapture onParticleCapture;

	// Token: 0x04004F0B RID: 20235
	public HighEnergyParticlePort.OnParticleCaptureAllowed onParticleCaptureAllowed;

	// Token: 0x04004F0C RID: 20236
	public HighEnergyParticlePort.OnParticleCapture onParticleUncapture;

	// Token: 0x04004F0D RID: 20237
	public HighEnergyParticle currentParticle;

	// Token: 0x04004F0E RID: 20238
	public bool requireOperational = true;

	// Token: 0x04004F0F RID: 20239
	public bool particleInputEnabled;

	// Token: 0x04004F10 RID: 20240
	public bool particleOutputEnabled;

	// Token: 0x04004F11 RID: 20241
	public CellOffset particleInputOffset;

	// Token: 0x04004F12 RID: 20242
	public CellOffset particleOutputOffset;

	// Token: 0x020013F0 RID: 5104
	// (Invoke) Token: 0x060068CA RID: 26826
	public delegate void OnParticleCapture(HighEnergyParticle particle);

	// Token: 0x020013F1 RID: 5105
	// (Invoke) Token: 0x060068CE RID: 26830
	public delegate bool OnParticleCaptureAllowed(HighEnergyParticle particle);
}
