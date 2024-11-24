using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A94 RID: 2708
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/MinimumOperatingTemperature")]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
{
	// Token: 0x06003203 RID: 12803 RVA: 0x000C0725 File Offset: 0x000BE925
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.TestTemperature(true);
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x000C0734 File Offset: 0x000BE934
	public void Sim200ms(float dt)
	{
		this.TestTemperature(false);
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x00201B30 File Offset: 0x001FFD30
	private void TestTemperature(bool force)
	{
		bool flag;
		if (this.primaryElement.Temperature < this.minimumTemperature)
		{
			flag = false;
		}
		else
		{
			flag = true;
			for (int i = 0; i < this.building.PlacementCells.Length; i++)
			{
				int i2 = this.building.PlacementCells[i];
				float num = Grid.Temperature[i2];
				float num2 = Grid.Mass[i2];
				if ((num != 0f || num2 != 0f) && num < this.minimumTemperature)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			this.lastOffTime = Time.time;
		}
		if ((flag != this.isWarm && !flag) || (flag != this.isWarm && flag && Time.time > this.lastOffTime + 5f) || force)
		{
			this.isWarm = flag;
			this.operational.SetFlag(MinimumOperatingTemperature.warmEnoughFlag, this.isWarm);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !this.isWarm, this);
		}
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x000C073D File Offset: 0x000BE93D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x00201C3C File Offset: 0x001FFE3C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x0400219C RID: 8604
	[MyCmpReq]
	private Building building;

	// Token: 0x0400219D RID: 8605
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400219E RID: 8606
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400219F RID: 8607
	public float minimumTemperature = 275.15f;

	// Token: 0x040021A0 RID: 8608
	private const float TURN_ON_DELAY = 5f;

	// Token: 0x040021A1 RID: 8609
	private float lastOffTime;

	// Token: 0x040021A2 RID: 8610
	public static readonly Operational.Flag warmEnoughFlag = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);

	// Token: 0x040021A3 RID: 8611
	private bool isWarm;

	// Token: 0x040021A4 RID: 8612
	private HandleVector<int>.Handle partitionerEntry;
}
