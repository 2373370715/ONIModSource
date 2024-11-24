using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001370 RID: 4976
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Generator")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer, ICircuitConnected
{
	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06006625 RID: 26149 RVA: 0x000E29CF File Offset: 0x000E0BCF
	public int PowerDistributionOrder
	{
		get
		{
			return this.powerDistributionOrder;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06006626 RID: 26150 RVA: 0x000E29D7 File Offset: 0x000E0BD7
	public virtual float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06006627 RID: 26151 RVA: 0x000E29DF File Offset: 0x000E0BDF
	public virtual bool IsEmpty
	{
		get
		{
			return this.joulesAvailable <= 0f;
		}
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06006628 RID: 26152 RVA: 0x000E29F1 File Offset: 0x000E0BF1
	public virtual float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06006629 RID: 26153 RVA: 0x000E29F9 File Offset: 0x000E0BF9
	public float WattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating * this.Efficiency;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x0600662A RID: 26154 RVA: 0x000E2A12 File Offset: 0x000E0C12
	public float BaseWattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating;
		}
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x0600662B RID: 26155 RVA: 0x000E2A24 File Offset: 0x000E0C24
	public float PercentFull
	{
		get
		{
			if (this.Capacity == 0f)
			{
				return 1f;
			}
			return this.joulesAvailable / this.Capacity;
		}
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x0600662C RID: 26156 RVA: 0x000E2A46 File Offset: 0x000E0C46
	// (set) Token: 0x0600662D RID: 26157 RVA: 0x000E2A4E File Offset: 0x000E0C4E
	public int PowerCell { get; private set; }

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x0600662E RID: 26158 RVA: 0x000C8892 File Offset: 0x000C6A92
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x0600662F RID: 26159 RVA: 0x000E2A57 File Offset: 0x000E0C57
	private float Efficiency
	{
		get
		{
			return Mathf.Max(1f + this.generatorOutputAttribute.GetTotalValue() / 100f, 0f);
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06006630 RID: 26160 RVA: 0x000E2A7A File Offset: 0x000E0C7A
	// (set) Token: 0x06006631 RID: 26161 RVA: 0x000E2A82 File Offset: 0x000E0C82
	public bool IsVirtual { get; protected set; }

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06006632 RID: 26162 RVA: 0x000E2A8B File Offset: 0x000E0C8B
	// (set) Token: 0x06006633 RID: 26163 RVA: 0x000E2A93 File Offset: 0x000E0C93
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06006634 RID: 26164 RVA: 0x002CE484 File Offset: 0x002CC684
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.generatorOutputAttribute = attributes.Add(Db.Get().Attributes.GeneratorOutput);
	}

	// Token: 0x06006635 RID: 26165 RVA: 0x002CE4C0 File Offset: 0x002CC6C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Generators.Add(this);
		base.Subscribe<Generator>(-1582839653, Generator.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.capacity = Generator.CalculateCapacity(this.building.Def, null);
		this.PowerCell = this.building.GetPowerOutputCell();
		this.CheckConnectionStatus();
		Game.Instance.energySim.AddGenerator(this);
	}

	// Token: 0x06006636 RID: 26166 RVA: 0x000E2A9C File Offset: 0x000E0C9C
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x06006637 RID: 26167 RVA: 0x000E2ACD File Offset: 0x000E0CCD
	public virtual bool IsProducingPower()
	{
		return this.operational.IsActive;
	}

	// Token: 0x06006638 RID: 26168 RVA: 0x000E2ADA File Offset: 0x000E0CDA
	public virtual void EnergySim200ms(float dt)
	{
		this.CheckConnectionStatus();
	}

	// Token: 0x06006639 RID: 26169 RVA: 0x002CE534 File Offset: 0x002CC734
	private void SetStatusItem(StatusItem status_item)
	{
		if (status_item != this.currentStatusItem && this.currentStatusItem != null)
		{
			this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID, false);
		}
		if (status_item != null && this.statusItemID == Guid.Empty)
		{
			this.statusItemID = this.selectable.AddStatusItem(status_item, this);
		}
		this.currentStatusItem = status_item;
	}

	// Token: 0x0600663A RID: 26170 RVA: 0x002CE59C File Offset: 0x002CC79C
	private void CheckConnectionStatus()
	{
		if (this.CircuitID == 65535)
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, false);
			return;
		}
		if (!Game.Instance.circuitManager.HasConsumers(this.CircuitID) && !Game.Instance.circuitManager.HasBatteries(this.CircuitID))
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, true);
			return;
		}
		this.SetStatusItem(null);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
	}

	// Token: 0x0600663B RID: 26171 RVA: 0x000E2AE2 File Offset: 0x000E0CE2
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveGenerator(this);
		Game.Instance.circuitManager.Disconnect(this);
		Components.Generators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600663C RID: 26172 RVA: 0x000E2B15 File Offset: 0x000E0D15
	public static float CalculateCapacity(BuildingDef def, Element element)
	{
		if (element == null)
		{
			return def.GeneratorBaseCapacity;
		}
		return def.GeneratorBaseCapacity * (1f + (element.HasTag(GameTags.RefinedMetal) ? 1f : 0f));
	}

	// Token: 0x0600663D RID: 26173 RVA: 0x000E2B47 File Offset: 0x000E0D47
	public void ResetJoules()
	{
		this.joulesAvailable = 0f;
	}

	// Token: 0x0600663E RID: 26174 RVA: 0x000E2B54 File Offset: 0x000E0D54
	public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
	{
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0f, canOverPower ? float.MaxValue : this.Capacity);
	}

	// Token: 0x0600663F RID: 26175 RVA: 0x002CE65C File Offset: 0x002CC85C
	public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
	{
		global::Debug.Assert(base.GetComponent<Battery>() == null);
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesAvailable, 0f, canOverPower ? float.MaxValue : this.Capacity);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, this.joulesAvailable, this.GetProperName(), null);
		if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.PrefabID()))
		{
			Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.PrefabID(), 0f);
		}
		Dictionary<Tag, float> powerCreatedbyGeneratorType = Game.Instance.savedInfo.powerCreatedbyGeneratorType;
		Tag key = this.PrefabID();
		powerCreatedbyGeneratorType[key] += this.joulesAvailable;
	}

	// Token: 0x06006640 RID: 26176 RVA: 0x000E2B7E File Offset: 0x000E0D7E
	public void AssignJoulesAvailable(float joulesAvailable)
	{
		global::Debug.Assert(base.GetComponent<PowerTransformer>() != null);
		this.joulesAvailable = joulesAvailable;
	}

	// Token: 0x06006641 RID: 26177 RVA: 0x000E2B98 File Offset: 0x000E0D98
	public virtual void ConsumeEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x04004CA6 RID: 19622
	protected const int SimUpdateSortKey = 1001;

	// Token: 0x04004CA7 RID: 19623
	[MyCmpReq]
	protected Building building;

	// Token: 0x04004CA8 RID: 19624
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04004CA9 RID: 19625
	[MyCmpReq]
	protected KSelectable selectable;

	// Token: 0x04004CAA RID: 19626
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04004CAB RID: 19627
	[SerializeField]
	public int powerDistributionOrder;

	// Token: 0x04004CAC RID: 19628
	public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04004CAD RID: 19629
	protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04004CAE RID: 19630
	private float capacity;

	// Token: 0x04004CB2 RID: 19634
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04004CB3 RID: 19635
	[SerializeField]
	public Tag[] connectedTags = Generator.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04004CB4 RID: 19636
	public bool showConnectedConsumerStatusItems = true;

	// Token: 0x04004CB5 RID: 19637
	private StatusItem currentStatusItem;

	// Token: 0x04004CB6 RID: 19638
	private Guid statusItemID;

	// Token: 0x04004CB7 RID: 19639
	private AttributeInstance generatorOutputAttribute;

	// Token: 0x04004CB8 RID: 19640
	private static readonly EventSystem.IntraObjectHandler<Generator> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Generator>(delegate(Generator component, object data)
	{
		component.OnTagsChanged(data);
	});
}
