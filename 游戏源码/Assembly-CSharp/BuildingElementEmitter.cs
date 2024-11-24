using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C71 RID: 3185
[AddComponentMenu("KMonoBehaviour/scripts/BuildingElementEmitter")]
public class BuildingElementEmitter : KMonoBehaviour, IGameObjectEffectDescriptor, IElementEmitter, ISim200ms
{
	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06003D1B RID: 15643 RVA: 0x000C78DC File Offset: 0x000C5ADC
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06003D1C RID: 15644 RVA: 0x000C78F3 File Offset: 0x000C5AF3
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06003D1D RID: 15645 RVA: 0x000C78FB File Offset: 0x000C5AFB
	public SimHashes Element
	{
		get
		{
			return this.element;
		}
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x000C7903 File Offset: 0x000C5B03
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		base.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
		this.SimRegister();
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x000C793D File Offset: 0x000C5B3D
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x000C7961 File Offset: 0x000C5B61
	private void OnActiveChanged(object data)
	{
		this.simActive = ((Operational)data).IsActive;
		this.dirty = true;
	}

	// Token: 0x06003D21 RID: 15649 RVA: 0x000C797B File Offset: 0x000C5B7B
	public void Sim200ms(float dt)
	{
		this.UnsafeUpdate(dt);
	}

	// Token: 0x06003D22 RID: 15650 RVA: 0x00230620 File Offset: 0x0022E820
	private unsafe void UnsafeUpdate(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
		int handleIndex = Sim.GetHandleIndex(this.simHandle);
		Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
		if (emittedMassInfo.mass > 0f)
		{
			Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
			if (this.element == SimHashes.Oxygen)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, base.gameObject.GetProperName(), null);
			}
		}
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x002306C0 File Offset: 0x0022E8C0
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			if (this.element != (SimHashes)0 && this.emitRate > 0f)
			{
				int game_cell = Grid.PosToCell(new Vector3(base.transform.GetPosition().x + this.modifierOffset.x, base.transform.GetPosition().y + this.modifierOffset.y, 0f));
				SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
			}
			this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, this);
			return;
		}
		SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, this);
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x002307F8 File Offset: 0x0022E9F8
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback), this, "BuildingElementEmitter").index, -1, -1);
		}
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x000C7984 File Offset: 0x000C5B84
	private void SimUnregister()
	{
		if (this.simHandle != -1)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				SimMessages.RemoveElementEmitter(-1, this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x000C79AF File Offset: 0x000C5BAF
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((BuildingElementEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x000C79BD File Offset: 0x000C5BBD
	private void OnSimRegistered(int handle)
	{
		if (this != null)
		{
			this.simHandle = handle;
			return;
		}
		SimMessages.RemoveElementEmitter(-1, handle);
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x00230854 File Offset: 0x0022EA54
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.element).tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x0400299B RID: 10651
	[SerializeField]
	public float emitRate = 0.3f;

	// Token: 0x0400299C RID: 10652
	[SerializeField]
	[Serialize]
	public float temperature = 293f;

	// Token: 0x0400299D RID: 10653
	[SerializeField]
	[HashedEnum]
	public SimHashes element = SimHashes.Oxygen;

	// Token: 0x0400299E RID: 10654
	[SerializeField]
	public Vector2 modifierOffset;

	// Token: 0x0400299F RID: 10655
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x040029A0 RID: 10656
	[SerializeField]
	public byte emitDiseaseIdx = byte.MaxValue;

	// Token: 0x040029A1 RID: 10657
	[SerializeField]
	public int emitDiseaseCount;

	// Token: 0x040029A2 RID: 10658
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040029A3 RID: 10659
	private int simHandle = -1;

	// Token: 0x040029A4 RID: 10660
	private bool simActive;

	// Token: 0x040029A5 RID: 10661
	private bool dirty = true;

	// Token: 0x040029A6 RID: 10662
	private Guid statusHandle;

	// Token: 0x040029A7 RID: 10663
	private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>(delegate(BuildingElementEmitter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
