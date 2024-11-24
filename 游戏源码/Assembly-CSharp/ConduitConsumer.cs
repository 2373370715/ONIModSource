using System;
using STRINGS;
using UnityEngine;

// Token: 0x020010D3 RID: 4307
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06005869 RID: 22633 RVA: 0x000D9A4D File Offset: 0x000D7C4D
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x0600586A RID: 22634 RVA: 0x000D9A55 File Offset: 0x000D7C55
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x0600586B RID: 22635 RVA: 0x000D9A5D File Offset: 0x000D7C5D
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null && this.m_buildingComplete != null;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x0600586C RID: 22636 RVA: 0x0028BC58 File Offset: 0x00289E58
	public bool CanConsume
	{
		get
		{
			bool result = false;
			if (this.IsConnected)
			{
				result = (this.GetConduitManager().GetContents(this.utilityCell).mass > 0f);
			}
			return result;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x0600586D RID: 22637 RVA: 0x0028BC94 File Offset: 0x00289E94
	public float stored_mass
	{
		get
		{
			if (this.storage == null)
			{
				return 0f;
			}
			if (!(this.capacityTag != GameTags.Any))
			{
				return this.storage.MassStored();
			}
			return this.storage.GetMassAvailable(this.capacityTag);
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x0600586E RID: 22638 RVA: 0x0028BCE4 File Offset: 0x00289EE4
	public float space_remaining_kg
	{
		get
		{
			float num = this.capacityKG - this.stored_mass;
			if (!(this.storage == null))
			{
				return Mathf.Min(this.storage.RemainingCapacity(), num);
			}
			return num;
		}
	}

	// Token: 0x0600586F RID: 22639 RVA: 0x000D9A94 File Offset: 0x000D7C94
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06005870 RID: 22640 RVA: 0x000D9A55 File Offset: 0x000D7C55
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06005871 RID: 22641 RVA: 0x000D9A9D File Offset: 0x000D7C9D
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06005872 RID: 22642 RVA: 0x000D9ABD File Offset: 0x000D7CBD
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06005873 RID: 22643 RVA: 0x000D9AE6 File Offset: 0x000D7CE6
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06005874 RID: 22644 RVA: 0x000D9AEE File Offset: 0x000D7CEE
	// (set) Token: 0x06005875 RID: 22645 RVA: 0x000D9B03 File Offset: 0x000D7D03
	public bool IsSatisfied
	{
		get
		{
			return this.satisfied || !this.isConsuming;
		}
		set
		{
			this.satisfied = (value || this.forceAlwaysSatisfied);
		}
	}

	// Token: 0x06005876 RID: 22646 RVA: 0x0028BD20 File Offset: 0x00289F20
	private ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06005877 RID: 22647 RVA: 0x0028BD58 File Offset: 0x00289F58
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06005878 RID: 22648 RVA: 0x0028BD88 File Offset: 0x00289F88
	protected virtual int GetInputCell(ConduitType inputConduitType)
	{
		if (this.useSecondaryInput)
		{
			ISecondaryInput[] components = base.GetComponents<ISecondaryInput>();
			foreach (ISecondaryInput secondaryInput in components)
			{
				if (secondaryInput.HasSecondaryConduitType(inputConduitType))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(inputConduitType));
				}
			}
			global::Debug.LogWarning("No secondaryInput of type was found");
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(inputConduitType));
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x06005879 RID: 22649 RVA: 0x0028BE08 File Offset: 0x0028A008
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x0600587A RID: 22650 RVA: 0x000D9B17 File Offset: 0x000D7D17
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600587B RID: 22651 RVA: 0x000D9B46 File Offset: 0x000D7D46
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x0600587C RID: 22652 RVA: 0x000D9B5E File Offset: 0x000D7D5E
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x0028BED4 File Offset: 0x0028A0D4
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x0600587E RID: 22654 RVA: 0x0028BF00 File Offset: 0x0028A100
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		if (this.building.Def.CanMove)
		{
			this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
		}
		if (!this.IsConnected)
		{
			return;
		}
		ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		this.IsSatisfied = true;
		if (!this.alwaysConsume && !this.operational.MeetsRequirements(this.OperatingRequirement))
		{
			return;
		}
		float num = this.ConsumptionRate * dt;
		num = Mathf.Min(num, this.space_remaining_kg);
		Element element = ElementLoader.FindElementByHash(contents.element);
		if (contents.element != this.lastConsumedElement)
		{
			DiscoveredResources.Instance.Discover(element.tag, element.materialCategory);
		}
		float num2 = 0f;
		if (num > 0f)
		{
			ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, num);
			num2 = conduitContents.mass;
			this.lastConsumedElement = conduitContents.element;
		}
		bool flag = element.HasTag(this.capacityTag);
		if (num2 > 0f && this.capacityTag != GameTags.Any && !flag)
		{
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
			});
		}
		if (flag || this.wrongElementResult == ConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
		{
			if (num2 > 0f)
			{
				this.consumedLastTick = true;
				int disease_count = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				Element element2 = ElementLoader.FindElementByHash(contents.element);
				ConduitType conduitType = this.conduitType;
				if (conduitType != ConduitType.Gas)
				{
					if (conduitType == ConduitType.Liquid)
					{
						if (element2.IsLiquid)
						{
							this.storage.AddLiquid(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
							return;
						}
						global::Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id.ToString());
						return;
					}
				}
				else
				{
					if (element2.IsGas)
					{
						this.storage.AddGasChunk(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
						return;
					}
					global::Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id.ToString());
					return;
				}
			}
		}
		else if (num2 > 0f)
		{
			this.consumedLastTick = true;
			if (this.wrongElementResult == ConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x04003E4A RID: 15946
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04003E4B RID: 15947
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x04003E4C RID: 15948
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04003E4D RID: 15949
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04003E4E RID: 15950
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x04003E4F RID: 15951
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x04003E50 RID: 15952
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x04003E51 RID: 15953
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04003E52 RID: 15954
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04003E53 RID: 15955
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x04003E54 RID: 15956
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x04003E55 RID: 15957
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04003E56 RID: 15958
	[MyCmpReq]
	protected Building building;

	// Token: 0x04003E57 RID: 15959
	public Operational.State OperatingRequirement;

	// Token: 0x04003E58 RID: 15960
	public ISecondaryInput targetSecondaryInput;

	// Token: 0x04003E59 RID: 15961
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04003E5A RID: 15962
	[MyCmpGet]
	private BuildingComplete m_buildingComplete;

	// Token: 0x04003E5B RID: 15963
	private int utilityCell = -1;

	// Token: 0x04003E5C RID: 15964
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x04003E5D RID: 15965
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x04003E5E RID: 15966
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003E5F RID: 15967
	private bool satisfied;

	// Token: 0x04003E60 RID: 15968
	public ConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x020010D4 RID: 4308
	public enum WrongElementResult
	{
		// Token: 0x04003E62 RID: 15970
		Destroy,
		// Token: 0x04003E63 RID: 15971
		Dump,
		// Token: 0x04003E64 RID: 15972
		Store
	}
}
