using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FDB RID: 4059
[AddComponentMenu("KMonoBehaviour/scripts/SweepBotStation")]
public class SweepBotStation : KMonoBehaviour
{
	// Token: 0x06005264 RID: 21092 RVA: 0x000D5BEF File Offset: 0x000D3DEF
	public void SetStorages(Storage botMaterialStorage, Storage sweepStorage)
	{
		this.botMaterialStorage = botMaterialStorage;
		this.sweepStorage = sweepStorage;
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x000D5BFF File Offset: 0x000D3DFF
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
		base.Subscribe<SweepBotStation>(-592767678, SweepBotStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x000D5C19 File Offset: 0x000D3E19
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, false);
	}

	// Token: 0x06005267 RID: 21095 RVA: 0x00275298 File Offset: 0x00273498
	protected override void OnSpawn()
	{
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		else
		{
			StorageUnloadMonitor.Instance smi = this.sweepBot.Get().GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			this.RefreshSweepBotSubscription();
		}
		this.UpdateMeter();
		this.UpdateNameDisplay();
	}

	// Token: 0x06005268 RID: 21096 RVA: 0x00275358 File Offset: 0x00273558
	private void RequestNewSweepBot(object data = null)
	{
		if (this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS) == null)
		{
			FetchList2 fetchList = new FetchList2(this.botMaterialStorage, Db.Get().ChoreTypes.Fetch);
			fetchList.Add(GameTags.RefinedMetal, null, SweepBotConfig.MASS, Operational.State.None);
			fetchList.Submit(null, true);
			return;
		}
		this.MakeNewSweepBot(null);
	}

	// Token: 0x06005269 RID: 21097 RVA: 0x002753C0 File Offset: 0x002735C0
	private void MakeNewSweepBot(object data = null)
	{
		if (this.newSweepyHandle.IsValid)
		{
			return;
		}
		if (this.botMaterialStorage.GetAmountAvailable(GameTags.RefinedMetal) < SweepBotConfig.MASS)
		{
			return;
		}
		PrimaryElement primaryElement = this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS);
		if (primaryElement == null)
		{
			return;
		}
		SimHashes sweepBotMaterial = primaryElement.ElementID;
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.botMaterialStorage.ConsumeAndGetDisease(sweepBotMaterial.CreateTag(), SweepBotConfig.MASS, out num, out disease, out temperature);
		this.UpdateMeter();
		this.newSweepyHandle = GameScheduler.Instance.Schedule("MakeSweepy", 2f, delegate(object obj)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("SweepBot"), Grid.CellToPos(Grid.CellRight(Grid.PosToCell(this.gameObject))), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			this.sweepBot = new Ref<KSelectable>(gameObject.GetComponent<KSelectable>());
			if (!string.IsNullOrEmpty(this.storedName))
			{
				this.sweepBot.Get().GetComponent<UserNameable>().SetName(this.storedName);
			}
			this.UpdateNameDisplay();
			StorageUnloadMonitor.Instance smi = gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			PrimaryElement component = this.sweepBot.Get().GetComponent<PrimaryElement>();
			component.ElementID = sweepBotMaterial;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from the material used for its creation");
			}
			this.RefreshSweepBotSubscription();
			this.newSweepyHandle.ClearScheduler();
		}, null, null);
		base.GetComponent<KBatchedAnimController>().Play("newsweepy", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600526A RID: 21098 RVA: 0x002754A4 File Offset: 0x002736A4
	private void RefreshSweepBotSubscription()
	{
		if (this.refreshSweepbotHandle != -1)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
			this.sweepBot.Get().Unsubscribe(this.sweepBotNameChangeHandle);
		}
		this.refreshSweepbotHandle = this.sweepBot.Get().Subscribe(1969584890, new Action<object>(this.RequestNewSweepBot));
		this.sweepBotNameChangeHandle = this.sweepBot.Get().Subscribe(1102426921, new Action<object>(this.UpdateStoredName));
	}

	// Token: 0x0600526B RID: 21099 RVA: 0x000D5C32 File Offset: 0x000D3E32
	private void UpdateStoredName(object data)
	{
		this.storedName = (string)data;
		this.UpdateNameDisplay();
	}

	// Token: 0x0600526C RID: 21100 RVA: 0x00275534 File Offset: 0x00273734
	private void UpdateNameDisplay()
	{
		if (string.IsNullOrEmpty(this.storedName))
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, ROBOTS.MODELS.SWEEPBOT.NAME));
		}
		else
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, this.storedName));
		}
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x0600526D RID: 21101 RVA: 0x000D5C46 File Offset: 0x000D3E46
	public void DockRobot(bool docked)
	{
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, docked);
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x002755A0 File Offset: 0x002737A0
	public void StartCharging()
	{
		base.GetComponent<KBatchedAnimController>().Queue("sleep_pre", KAnim.PlayMode.Once, 1f, 0f);
		base.GetComponent<KBatchedAnimController>().Queue("sleep_idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x000D5C59 File Offset: 0x000D3E59
	public void StopCharging()
	{
		base.GetComponent<KBatchedAnimController>().Play("sleep_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.UpdateNameDisplay();
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x002755F0 File Offset: 0x002737F0
	protected override void OnCleanUp()
	{
		if (this.newSweepyHandle.IsValid)
		{
			this.newSweepyHandle.ClearScheduler();
		}
		if (this.refreshSweepbotHandle != -1 && this.sweepBot.Get() != null)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
		}
	}

	// Token: 0x06005271 RID: 21105 RVA: 0x00275648 File Offset: 0x00273848
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06005272 RID: 21106 RVA: 0x00275680 File Offset: 0x00273880
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component.currentFrame >= component.GetCurrentNumFrames())
		{
			base.GetComponent<KBatchedAnimController>().Play("remove", KAnim.PlayMode.Once, 1f, 0f);
		}
		for (int i = 0; i < this.sweepStorage.Count; i++)
		{
			this.sweepStorage[i].GetComponent<Clearable>().MarkForClear(false, true);
		}
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x000D5C81 File Offset: 0x000D3E81
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(!component.Flags.ContainsValue(false), false);
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
	}

	// Token: 0x06005274 RID: 21108 RVA: 0x000D5CC0 File Offset: 0x000D3EC0
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.sweepStorage.Capacity() - this.sweepStorage.storageFullMargin;
	}

	// Token: 0x06005275 RID: 21109 RVA: 0x000D5CD9 File Offset: 0x000D3ED9
	private float GetAmountStored()
	{
		return this.sweepStorage.MassStored();
	}

	// Token: 0x0400399C RID: 14748
	[Serialize]
	public Ref<KSelectable> sweepBot;

	// Token: 0x0400399D RID: 14749
	[Serialize]
	public string storedName;

	// Token: 0x0400399E RID: 14750
	private static readonly Operational.Flag dockedRobot = new Operational.Flag("dockedRobot", Operational.Flag.Type.Functional);

	// Token: 0x0400399F RID: 14751
	private MeterController meter;

	// Token: 0x040039A0 RID: 14752
	[SerializeField]
	private Storage botMaterialStorage;

	// Token: 0x040039A1 RID: 14753
	[SerializeField]
	private Storage sweepStorage;

	// Token: 0x040039A2 RID: 14754
	private SchedulerHandle newSweepyHandle;

	// Token: 0x040039A3 RID: 14755
	private static readonly EventSystem.IntraObjectHandler<SweepBotStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SweepBotStation>(delegate(SweepBotStation component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040039A4 RID: 14756
	private int refreshSweepbotHandle = -1;

	// Token: 0x040039A5 RID: 14757
	private int sweepBotNameChangeHandle = -1;
}
