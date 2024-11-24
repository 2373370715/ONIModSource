using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class DirectVolumeHeater : KMonoBehaviour, ISim33ms, ISim200ms, ISim1000ms, ISim4000ms, IGameObjectEffectDescriptor
{
	private enum TimeMode
	{
		ms33,
		ms200,
		ms1000,
		ms4000
	}

	[SerializeField]
	public int width = 12;

	[SerializeField]
	public int height = 4;

	[SerializeField]
	public float DTUs = 100000f;

	[SerializeField]
	public float maximumInternalTemperature = 773.15f;

	[SerializeField]
	public float maximumExternalTemperature = 340f;

	[SerializeField]
	public Operational operational;

	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	public bool EnableEmission;

	private HandleVector<int>.Handle structureTemperature;

	private PrimaryElement primaryElement;

	[SerializeField]
	private TimeMode impulseFrequency = TimeMode.ms1000;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		primaryElement = GetComponent<PrimaryElement>();
		structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	public void Sim33ms(float dt)
	{
		if (impulseFrequency == TimeMode.ms33)
		{
			float num = 0f;
			num += AddHeatToVolume(dt);
			num += AddSelfHeat(dt);
			heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	public void Sim200ms(float dt)
	{
		if (impulseFrequency == TimeMode.ms200)
		{
			float num = 0f;
			num += AddHeatToVolume(dt);
			num += AddSelfHeat(dt);
			heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	public void Sim1000ms(float dt)
	{
		if (impulseFrequency == TimeMode.ms1000)
		{
			float num = 0f;
			num += AddHeatToVolume(dt);
			num += AddSelfHeat(dt);
			heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	public void Sim4000ms(float dt)
	{
		if (impulseFrequency == TimeMode.ms4000)
		{
			float num = 0f;
			num += AddHeatToVolume(dt);
			num += AddSelfHeat(dt);
			heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	private float CalculateCellWeight(int dx, int dy, int maxDistance)
	{
		return 1f + (float)(maxDistance - Math.Abs(dx) - Math.Abs(dy));
	}

	private bool TestLineOfSight(int offsetCell)
	{
		int cell = Grid.PosToCell(base.gameObject);
		Grid.CellToXY(offsetCell, out var x, out var y);
		Grid.CellToXY(cell, out var x2, out var y2);
		return Grid.FastTestLineOfSightSolid(x2, y2, x, y);
	}

	private float AddSelfHeat(float dt)
	{
		if (!EnableEmission)
		{
			return 0f;
		}
		if (primaryElement.Temperature > maximumInternalTemperature)
		{
			return 0f;
		}
		GameComps.StructureTemperatures.ProduceEnergy(structureTemperature, 8f * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return 8f;
	}

	private float AddHeatToVolume(float dt)
	{
		if (!EnableEmission)
		{
			return 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		int num2 = width / 2;
		int num3 = width % 2;
		int maxDistance = num2 + height;
		float num4 = 0f;
		float num5 = DTUs * dt / 1000f;
		for (int i = -num2; i < num2 + num3; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (Grid.IsCellOffsetValid(num, i, j))
				{
					int num6 = Grid.OffsetCell(num, i, j);
					if (!Grid.Solid[num6] && Grid.Mass[num6] != 0f && Grid.WorldIdx[num6] == Grid.WorldIdx[num] && TestLineOfSight(num6) && !(Grid.Temperature[num6] >= maximumExternalTemperature))
					{
						num4 += CalculateCellWeight(i, j, maxDistance);
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
			for (int l = 0; l < height; l++)
			{
				if (Grid.IsCellOffsetValid(num, k, l))
				{
					int num9 = Grid.OffsetCell(num, k, l);
					if (!Grid.Solid[num9] && Grid.Mass[num9] != 0f && Grid.WorldIdx[num9] == Grid.WorldIdx[num] && TestLineOfSight(num9) && !(Grid.Temperature[num9] >= maximumExternalTemperature))
					{
						float num10 = num7 * CalculateCellWeight(k, l, maxDistance);
						num8 += num10;
						SimMessages.ModifyEnergy(num9, num10, 10000f, SimMessages.EnergySourceID.HeatBulb);
					}
				}
			}
		}
		return num8;
	}

	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(DTUs);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy));
		list.Add(item);
		return list;
	}
}
