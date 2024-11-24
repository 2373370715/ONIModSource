using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020019B9 RID: 6585
public struct StructureTemperaturePayload
{
	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x0600891E RID: 35102 RVA: 0x000F9BFB File Offset: 0x000F7DFB
	// (set) Token: 0x0600891F RID: 35103 RVA: 0x000F9C03 File Offset: 0x000F7E03
	public PrimaryElement primaryElement
	{
		get
		{
			return this.primaryElementBacking;
		}
		set
		{
			if (this.primaryElementBacking != value)
			{
				this.primaryElementBacking = value;
				this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
			}
		}
	}

	// Token: 0x06008920 RID: 35104 RVA: 0x0035617C File Offset: 0x0035437C
	public StructureTemperaturePayload(GameObject go)
	{
		this.simHandleCopy = -1;
		this.enabled = true;
		this.bypass = false;
		this.overrideExtents = false;
		this.overriddenExtents = default(Extents);
		this.primaryElementBacking = go.GetComponent<PrimaryElement>();
		this.overheatable = ((this.primaryElementBacking != null) ? this.primaryElementBacking.GetComponent<Overheatable>() : null);
		this.building = go.GetComponent<Building>();
		this.operational = go.GetComponent<Operational>();
		this.heatEffect = go.GetComponent<KBatchedAnimHeatPostProcessingEffect>();
		this.pendingEnergyModifications = 0f;
		this.maxTemperature = 10000f;
		this.energySourcesKW = null;
		this.isActiveStatusItemSet = false;
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06008921 RID: 35105 RVA: 0x00356228 File Offset: 0x00354428
	public float TotalEnergyProducedKW
	{
		get
		{
			if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.energySourcesKW.Count; i++)
			{
				num += this.energySourcesKW[i].value;
			}
			return num;
		}
	}

	// Token: 0x06008922 RID: 35106 RVA: 0x000F9C2B File Offset: 0x000F7E2B
	public void OverrideExtents(Extents newExtents)
	{
		this.overrideExtents = true;
		this.overriddenExtents = newExtents;
	}

	// Token: 0x06008923 RID: 35107 RVA: 0x000F9C3B File Offset: 0x000F7E3B
	public Extents GetExtents()
	{
		if (!this.overrideExtents)
		{
			return this.building.GetExtents();
		}
		return this.overriddenExtents;
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06008924 RID: 35108 RVA: 0x000F9C57 File Offset: 0x000F7E57
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06008925 RID: 35109 RVA: 0x000F9C64 File Offset: 0x000F7E64
	public float ExhaustKilowatts
	{
		get
		{
			return this.building.Def.ExhaustKilowattsWhenActive;
		}
	}

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06008926 RID: 35110 RVA: 0x000F9C76 File Offset: 0x000F7E76
	public float OperatingKilowatts
	{
		get
		{
			if (!(this.operational != null) || !this.operational.IsActive)
			{
				return 0f;
			}
			return this.building.Def.SelfHeatKilowattsWhenActive;
		}
	}

	// Token: 0x04006740 RID: 26432
	public int simHandleCopy;

	// Token: 0x04006741 RID: 26433
	public bool enabled;

	// Token: 0x04006742 RID: 26434
	public bool bypass;

	// Token: 0x04006743 RID: 26435
	public bool isActiveStatusItemSet;

	// Token: 0x04006744 RID: 26436
	public bool overrideExtents;

	// Token: 0x04006745 RID: 26437
	private PrimaryElement primaryElementBacking;

	// Token: 0x04006746 RID: 26438
	public Overheatable overheatable;

	// Token: 0x04006747 RID: 26439
	public Building building;

	// Token: 0x04006748 RID: 26440
	public Operational operational;

	// Token: 0x04006749 RID: 26441
	public KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x0400674A RID: 26442
	public List<StructureTemperaturePayload.EnergySource> energySourcesKW;

	// Token: 0x0400674B RID: 26443
	public float pendingEnergyModifications;

	// Token: 0x0400674C RID: 26444
	public float maxTemperature;

	// Token: 0x0400674D RID: 26445
	public Extents overriddenExtents;

	// Token: 0x020019BA RID: 6586
	public class EnergySource
	{
		// Token: 0x06008927 RID: 35111 RVA: 0x000F9CA9 File Offset: 0x000F7EA9
		public EnergySource(float kj, string source)
		{
			this.source = source;
			this.kw_accumulator = new RunningAverage(float.MinValue, float.MaxValue, Mathf.RoundToInt(186f), true);
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06008928 RID: 35112 RVA: 0x000F9CD8 File Offset: 0x000F7ED8
		public float value
		{
			get
			{
				return this.kw_accumulator.AverageValue;
			}
		}

		// Token: 0x06008929 RID: 35113 RVA: 0x000F9CE5 File Offset: 0x000F7EE5
		public void Accumulate(float value)
		{
			this.kw_accumulator.AddSample(value);
		}

		// Token: 0x0400674E RID: 26446
		public string source;

		// Token: 0x0400674F RID: 26447
		public RunningAverage kw_accumulator;
	}
}
