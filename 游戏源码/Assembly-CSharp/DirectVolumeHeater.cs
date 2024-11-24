using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D38 RID: 3384
public class DirectVolumeHeater : KMonoBehaviour, ISim33ms, ISim200ms, ISim1000ms, ISim4000ms, IGameObjectEffectDescriptor
{
	// Token: 0x06004231 RID: 16945 RVA: 0x000CAC97 File Offset: 0x000C8E97
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x06004232 RID: 16946 RVA: 0x0023FF8C File Offset: 0x0023E18C
	public void Sim33ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms33)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06004233 RID: 16947 RVA: 0x0023FFC8 File Offset: 0x0023E1C8
	public void Sim200ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms200)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x00240004 File Offset: 0x0023E204
	public void Sim1000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms1000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06004235 RID: 16949 RVA: 0x00240040 File Offset: 0x0023E240
	public void Sim4000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms4000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06004236 RID: 16950 RVA: 0x000CACC1 File Offset: 0x000C8EC1
	private float CalculateCellWeight(int dx, int dy, int maxDistance)
	{
		return 1f + (float)(maxDistance - Math.Abs(dx) - Math.Abs(dy));
	}

	// Token: 0x06004237 RID: 16951 RVA: 0x0024007C File Offset: 0x0023E27C
	private bool TestLineOfSight(int offsetCell)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int x;
		int y;
		Grid.CellToXY(offsetCell, out x, out y);
		int x2;
		int y2;
		Grid.CellToXY(cell, out x2, out y2);
		return Grid.FastTestLineOfSightSolid(x2, y2, x, y);
	}

	// Token: 0x06004238 RID: 16952 RVA: 0x002400B0 File Offset: 0x0023E2B0
	private float AddSelfHeat(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		if (this.primaryElement.Temperature > this.maximumInternalTemperature)
		{
			return 0f;
		}
		float result = 8f;
		GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 8f * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return result;
	}

	// Token: 0x06004239 RID: 16953 RVA: 0x0024010C File Offset: 0x0023E30C
	private float AddHeatToVolume(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		int num2 = this.width / 2;
		int num3 = this.width % 2;
		int maxDistance = num2 + this.height;
		float num4 = 0f;
		float num5 = this.DTUs * dt / 1000f;
		for (int i = -num2; i < num2 + num3; i++)
		{
			for (int j = 0; j < this.height; j++)
			{
				if (Grid.IsCellOffsetValid(num, i, j))
				{
					int num6 = Grid.OffsetCell(num, i, j);
					if (!Grid.Solid[num6] && Grid.Mass[num6] != 0f && Grid.WorldIdx[num6] == Grid.WorldIdx[num] && this.TestLineOfSight(num6) && Grid.Temperature[num6] < this.maximumExternalTemperature)
					{
						num4 += this.CalculateCellWeight(i, j, maxDistance);
					}
				}
			}
		}
		float num7 = num5;
		if (num4 > 0f)
		{
			num7 /= num4;
		}
		float num8 = 0f;
		for (int k = -num2; k < num2 + num3; k++)
		{
			for (int l = 0; l < this.height; l++)
			{
				if (Grid.IsCellOffsetValid(num, k, l))
				{
					int num9 = Grid.OffsetCell(num, k, l);
					if (!Grid.Solid[num9] && Grid.Mass[num9] != 0f && Grid.WorldIdx[num9] == Grid.WorldIdx[num] && this.TestLineOfSight(num9) && Grid.Temperature[num9] < this.maximumExternalTemperature)
					{
						float num10 = num7 * this.CalculateCellWeight(k, l, maxDistance);
						num8 += num10;
						SimMessages.ModifyEnergy(num9, num10, 10000f, SimMessages.EnergySourceID.HeatBulb);
					}
				}
			}
		}
		return num8;
	}

	// Token: 0x0600423A RID: 16954 RVA: 0x002402F4 File Offset: 0x0023E4F4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.DTUs, GameUtil.HeatEnergyFormatterUnit.Automatic);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04002D0E RID: 11534
	[SerializeField]
	public int width = 12;

	// Token: 0x04002D0F RID: 11535
	[SerializeField]
	public int height = 4;

	// Token: 0x04002D10 RID: 11536
	[SerializeField]
	public float DTUs = 100000f;

	// Token: 0x04002D11 RID: 11537
	[SerializeField]
	public float maximumInternalTemperature = 773.15f;

	// Token: 0x04002D12 RID: 11538
	[SerializeField]
	public float maximumExternalTemperature = 340f;

	// Token: 0x04002D13 RID: 11539
	[SerializeField]
	public Operational operational;

	// Token: 0x04002D14 RID: 11540
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04002D15 RID: 11541
	public bool EnableEmission;

	// Token: 0x04002D16 RID: 11542
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04002D17 RID: 11543
	private PrimaryElement primaryElement;

	// Token: 0x04002D18 RID: 11544
	[SerializeField]
	private DirectVolumeHeater.TimeMode impulseFrequency = DirectVolumeHeater.TimeMode.ms1000;

	// Token: 0x02000D39 RID: 3385
	private enum TimeMode
	{
		// Token: 0x04002D1A RID: 11546
		ms33,
		// Token: 0x04002D1B RID: 11547
		ms200,
		// Token: 0x04002D1C RID: 11548
		ms1000,
		// Token: 0x04002D1D RID: 11549
		ms4000
	}
}
