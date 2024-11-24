using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001299 RID: 4761
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SpawnableConduitConsumer")]
public class EntityConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060061F3 RID: 25075 RVA: 0x000DFEED File Offset: 0x000DE0ED
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060061F4 RID: 25076 RVA: 0x000DFEF5 File Offset: 0x000DE0F5
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060061F5 RID: 25077 RVA: 0x000DFEFD File Offset: 0x000DE0FD
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x060061F6 RID: 25078 RVA: 0x002B4B58 File Offset: 0x002B2D58
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

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x060061F7 RID: 25079 RVA: 0x002B4B94 File Offset: 0x002B2D94
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

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x060061F8 RID: 25080 RVA: 0x002B4BE4 File Offset: 0x002B2DE4
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

	// Token: 0x060061F9 RID: 25081 RVA: 0x000DFF24 File Offset: 0x000DE124
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x060061FA RID: 25082 RVA: 0x000DFEF5 File Offset: 0x000DE0F5
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x060061FB RID: 25083 RVA: 0x000DFF2D File Offset: 0x000DE12D
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x060061FC RID: 25084 RVA: 0x000DFF4D File Offset: 0x000DE14D
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x060061FD RID: 25085 RVA: 0x000DFF76 File Offset: 0x000DE176
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x060061FE RID: 25086 RVA: 0x000DFF7E File Offset: 0x000DE17E
	// (set) Token: 0x060061FF RID: 25087 RVA: 0x000DFF93 File Offset: 0x000DE193
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

	// Token: 0x06006200 RID: 25088 RVA: 0x002B4C20 File Offset: 0x002B2E20
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

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06006201 RID: 25089 RVA: 0x002B4C58 File Offset: 0x002B2E58
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x000DFFA7 File Offset: 0x000DE1A7
	private int GetInputCell(ConduitType inputConduitType)
	{
		return this.occupyArea.GetOffsetCellWithRotation(this.offset);
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x002B4C88 File Offset: 0x002B2E88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.endpoint = new FlowUtilityNetwork.NetworkItem(conduitManager.conduitType, Endpoint.Sink, this.utilityCell, base.gameObject);
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(conduitManager.conduitType).AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		EntityCellVisualizer.Ports type = EntityCellVisualizer.Ports.LiquidIn;
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			type = EntityCellVisualizer.Ports.SolidIn;
		}
		else if (conduitManager.conduitType == ConduitType.Gas)
		{
			type = EntityCellVisualizer.Ports.GasIn;
		}
		this.cellVisualizer.AddPort(type, this.offset);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x002B4DAC File Offset: 0x002B2FAC
	protected override void OnCleanUp()
	{
		if (this.endpoint.ConduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(this.endpoint.ConduitType).RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06006205 RID: 25093 RVA: 0x000DFFBA File Offset: 0x000DE1BA
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06006206 RID: 25094 RVA: 0x000DFFD2 File Offset: 0x000DE1D2
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06006207 RID: 25095 RVA: 0x002B4E40 File Offset: 0x002B3040
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x06006208 RID: 25096 RVA: 0x002B4E6C File Offset: 0x002B306C
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
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
		if (flag || this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
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
			if (this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x040045AF RID: 17839
	private FlowUtilityNetwork.NetworkItem endpoint;

	// Token: 0x040045B0 RID: 17840
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x040045B1 RID: 17841
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x040045B2 RID: 17842
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x040045B3 RID: 17843
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040045B4 RID: 17844
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x040045B5 RID: 17845
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x040045B6 RID: 17846
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x040045B7 RID: 17847
	[SerializeField]
	public bool isOn = true;

	// Token: 0x040045B8 RID: 17848
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x040045B9 RID: 17849
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x040045BA RID: 17850
	[MyCmpReq]
	public Operational operational;

	// Token: 0x040045BB RID: 17851
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040045BC RID: 17852
	[MyCmpReq]
	private EntityCellVisualizer cellVisualizer;

	// Token: 0x040045BD RID: 17853
	public Operational.State OperatingRequirement;

	// Token: 0x040045BE RID: 17854
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040045BF RID: 17855
	public CellOffset offset;

	// Token: 0x040045C0 RID: 17856
	private int utilityCell = -1;

	// Token: 0x040045C1 RID: 17857
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x040045C2 RID: 17858
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x040045C3 RID: 17859
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040045C4 RID: 17860
	private bool satisfied;

	// Token: 0x040045C5 RID: 17861
	public EntityConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x0200129A RID: 4762
	public enum WrongElementResult
	{
		// Token: 0x040045C7 RID: 17863
		Destroy,
		// Token: 0x040045C8 RID: 17864
		Dump,
		// Token: 0x040045C9 RID: 17865
		Store
	}
}
